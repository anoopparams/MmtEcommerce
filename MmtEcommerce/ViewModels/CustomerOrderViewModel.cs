using System.Linq;
using System.Threading.Tasks;

namespace MmtEcommerce.ViewModels
{
    public class CustomerOrderViewModel
    {
        public CustomerViewModel Customer { get; set; }
        public OrderViewModel Order { get; set; }
    }
}
