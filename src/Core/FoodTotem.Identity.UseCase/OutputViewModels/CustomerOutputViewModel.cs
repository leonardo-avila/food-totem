namespace FoodTotem.Identity.UseCase.OutputViewModels
{
	public class CustomerOutputViewModel
	{
		public Guid Id { get; set; }
		public string Identification { get; set; }
		public string AuthenticationType { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
	}
}

