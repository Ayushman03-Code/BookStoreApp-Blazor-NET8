namespace BookStoreApp.API.Models
{
    public class BaseDto
    {
        // we need the id feild to be present in UpdateAuthorDto but dont need to show it therefore we need base dto from which it will inherit it.
        // So every entity that a dto is modelling will have an id atleast based on the way we have designed our database
        // So this is a base way of saying that everybody is going to have an Id once you ingerit from BaseDto
        // This should include every base property every DTO should have 
        public int Id { get; set; }
    }
}
