using System.Collections.Generic;
using System.Linq;
using Dapper;
using ui.Models;

namespace ui.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(string getTableName):base(getTableName)
        {
            
        }


        public IEnumerable<Order> GetCustomerOrders(int customerId)
        {
            var sql = "SELECT * FROM orders as o " +
            "Inner JOIN ordered_vehicles as v on o.order_number = v.order_number "+
            "Inner JOIN customers as c on o.customer_number = c.customer_number "+
            "Inner JOIN employees as e on o.employee_number = e.employee_number "+
            "Inner JOIN branches as b on e.branch_name = b.branch_name "+
            "Inner JOIN models as m on v.model_name = m.model_name "+
            "Inner JOIN trims as t on v.trim_name = t.trim_name "+
            "Inner JOIN trim_equipment as te on t.trim_name = te.trim_name "+
            "Inner JOIN engines as en on v.engine_designation = en.engine_designation ";




            //var orders = _dbConnection.Query<Order, Customer, Employee, OrderedVehicles, Order>(
            //        sql,
            //        (o, c, e, v) =>
            //        {
            //            o.Customer = c;
            //            o.Employee = e;
            //            o.OrderedVehicles = v;
            //            return o;
            //        },
            //        splitOn: "customer_number")
            //    .Distinct()
            //    .ToList();

            return _dbConnection.Query<Order>(sql).ToList();
            //return orders;
        }
    }
}