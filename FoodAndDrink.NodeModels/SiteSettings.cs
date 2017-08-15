namespace FoodAndDrink.NodeModels
{
    public class SiteSettings: Node
    {
        [SettingsSmtpProcessor]
        public SettingsSmtp Smtp { get; set; }

        [SettingEmailTemplateProcessor]
        public SettingEmailTemplate EmailTemplate { get; set; }
    }
}
