$(document).ready(function() {
    if (document.querySelector('.cron-input')) {
        const valueHolder = document.querySelector('.cron-input + .cron-input_value');
        $('.cron-input').cron({
            initial: valueHolder.value,
            onChange: function() {
                valueHolder.value = $(this).cron("value");
            },
            customValues: {
                "every hour": "0 * * * *",
                "every 2 hours": "0 */2 * * *",
                "every 3 hours": "0 */3 * * *",
                "every 12 hours": "0 */12 * * *",
            }
        });
    }
});

document.querySelector('.input__stretching').addEventListener('input', onStretchingInput);

function onStretchingInput(){
    this.style.width = '0px';
    this.style.width = `${this.scrollWidth}px`;
}

document.querySelector('#NeedToSendEmails').addEventListener('input', onMailingCheckboxSwitch);

function onMailingCheckboxSwitch() {
    document.querySelector('#MailingOptions').toggleAttribute("hidden");
}
