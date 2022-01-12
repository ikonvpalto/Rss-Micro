using System;
using System.ComponentModel.DataAnnotations;

namespace Front.ViewModels;

public sealed class RssSubscriptionViewModel
{
    [Required]
    public Guid Guid { get; set; }

    public string Periodicity { get; set; } = "0 8 * * *";

    [Required]
    [Display(Name = "Enable mailing", Description = "Enable sending news on email")]
    public bool NeedToSendEmails { get; set; }

    [Required]
    [Url]
    public string RssSource { get; set; }

    [RegularExpression(@"[\w\-#]+")]
    public string Filters { get; set; } = string.Empty;

    public string Receivers { get; set; } = string.Empty;
}
