using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Order.Queries.GetOrderByName
{
  public record GetOrderByNameQuery(string Name) : IQuery<GetOrderByNameResult>;
  public record GetOrderByNameResult (IEnumerable<OrdersDTO> Orders);

}
