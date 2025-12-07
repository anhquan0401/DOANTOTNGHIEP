namespace WebApplication1.ViewModels
{
    public class ChatbotResponseVM
    {
        public string llm_answers { get; set; }
        public List<RecommendProduct> related_products { get; set; }
        //public List<ProductVM> SameCategoryProducts { get; set; }
        public string Error { get; set; }
    }
}
