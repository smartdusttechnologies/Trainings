using System.ComponentModel.DataAnnotations;

namespace Signal_R.Models
{
   
        public class OrderPreparation
        {
      
          public Order Order { get; set; }
            public Rider Rider { get; set; }
        
    }
}
