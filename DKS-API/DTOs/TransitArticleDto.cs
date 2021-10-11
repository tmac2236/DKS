namespace DKS_API.DTOs
{
    public class TransitArticleDto
    {

        public string Article { get; set; }
        public string ModelNo { get; set; }
        public string ModelNoFrom { get; set; }
        public string DevTeamId { get; set; }
        public string FactoryId { get; set; }
        public string FactoryIdFrom { get; set; } 
        public string PkArticle {get;set;}
        public string Stage {get;set;}

        public string UpdateUser {get;set;}
    }
}