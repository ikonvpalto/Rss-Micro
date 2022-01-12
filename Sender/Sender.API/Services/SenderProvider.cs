using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using Common.Exceptions;
using Sender.API.Repository;
using Sender.API.Resources;
using Sender.Common.Contracts;
using Sender.Common.Models;

namespace Sender.API.Services
{
    public sealed class SenderProvider : ISenderProvider
    {
        private readonly IMapper _mapper;
        private readonly IEmailRepository _emailRepository;

        public SenderProvider(IMapper mapper, IEmailRepository emailRepository)
        {
            _mapper = mapper;
            _emailRepository = emailRepository;
        }

        public async Task<ReceiversModel> GetAsync(Guid receiverGuid)
        {
            var models = await _emailRepository.GetGroupAsync(receiverGuid);
            var receivers = _mapper.Map<ReceiversModel>(models);
            receivers.Guid = receiverGuid;
            return receivers;
        }

        public async Task<IEnumerable<ReceiversModel>> GetAsync()
        {
            var models = await _emailRepository.GetAsync();
            return models
                .GroupBy(m => m.GroupGuid)
                .Select(m => _mapper.Map<ReceiversModel>(m));
        }

        public Task EnsureReceiversIsValidAsync(IEnumerable<string> receiverEmails)
        {
            foreach (var email in receiverEmails)
            {
                try {
                    var address = new MailAddress(email).Address;
                } catch(FormatException e) {
                    return Task.FromException(new BadRequestException(string.Format(Localization.NotAnEmail, email), e));
                }
            }

            return Task.CompletedTask;
        }
    }
}
