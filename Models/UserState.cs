namespace MestreDigital.Model
{
    public class UserState
    {
        public long ChatId { get; set; }
        public ConversationStage CurrentStage { get; set; }
        public int? SelectedCategoryId { get; set; }
        public int? SelectedSubcategoryId { get; set; }
    }

    public enum ConversationStage
    {
        MainMenu,
        FAQs,
        CategorySelection,
        SubcategorySelection,
        ContentSelection
    }

 


}
