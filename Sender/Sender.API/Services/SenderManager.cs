using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Exceptions;
using Common.Extensions;
using Downloader.Common.Models;
using Sender.API.ExternalServices;
using Sender.API.Models;
using Sender.API.Repository;
using Sender.API.Resources;
using Sender.Common.Contracts;
using Sender.Common.Models;

namespace Sender.API.Services
{
    public sealed class SenderManager : ISenderManager
    {
        private readonly IMapper _mapper;
        private readonly IEmailRepository _emailRepository;
        private readonly ISenderProvider _senderProvider;
        private readonly ISmtpService _smtpService;

        public SenderManager(
            IMapper mapper,
            IEmailRepository emailRepository,
            ISenderProvider senderProvider,
            ISmtpService smtpService)
        {
            _mapper = mapper;
            _emailRepository = emailRepository;
            _senderProvider = senderProvider;
            _smtpService = smtpService;
        }

        public async Task CreateAsync(ReceiversModel receivers)
        {
            if (await _emailRepository.IsExistsAsync(f => f.GroupGuid == receivers.Guid).ConfigureAwait(false))
            {
                throw new AlreadyExistsException(Localization.ReceiverAlreadyExists);
            }

            await _senderProvider.EnsureReceiversIsValidAsync(receivers.Receivers).ConfigureAwait(false);

            var models = receivers.Receivers.Select(f =>
            {
                var model = _mapper.Map<ReceiverEmailModel>(receivers);
                model.Email = f;
                return model;
            });

            await _emailRepository.CreateAsync(models).ConfigureAwait(false);
        }

        public async Task UpdateAsync(ReceiversModel receivers)
        {
            await _senderProvider.EnsureReceiversIsValidAsync(receivers.Receivers).ConfigureAwait(false);

            await _emailRepository.DeleteGroupAsync(receivers.Guid).ConfigureAwait(false);

            await CreateAsync(receivers).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid receiversGuid)
        {
            if (!await _emailRepository.IsExistsAsync(f => f.GroupGuid == receiversGuid).ConfigureAwait(false))
            {
                throw new NotFoundException(Localization.ReceiverNotFound);
            }

            await _emailRepository.DeleteGroupAsync(receiversGuid).ConfigureAwait(false);
        }

        public async Task SendNewsAsync(Guid receiversGuid, IEnumerable<NewsItem> news)
        {
            var receivers = await _emailRepository.GetGroupAsync(receiversGuid).ConfigureAwait(false);
            var emails = receivers.Select(r => r.Email);
            var body = news.Select(n => $"{n.Title}\n</br></br>\n{n.Description}").JoinString("\n</br>\n");
            await _smtpService.SendMailAsync(emails, Localization.EmailSubject, body).ConfigureAwait(false);
        }
    }
}
