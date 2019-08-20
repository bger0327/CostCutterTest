using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ui.Models;

namespace ui.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private IRepository<Branch> _branchRepository;

        public IRepository<Branch> BranchRepository
        {
            get => _branchRepository ?? (_branchRepository = new Repository<Branch>(Branch.GetTableName()));
        }

        private IRepository<ModelsEnginesIntersect> _modelsEnginesIntersectRepository;

        public IRepository<ModelsEnginesIntersect> ModelsEnginesIntersectRepository
        {
            get => _modelsEnginesIntersectRepository ?? (_modelsEnginesIntersectRepository = new Repository<ModelsEnginesIntersect>(ModelsEnginesIntersect.GetTableName()));
        }

        private IRepository<OrderedVehicles> _orderedVehiclesRepository;

        public IRepository<OrderedVehicles> OrderedVehiclesRepository
        {
            get => _orderedVehiclesRepository ?? (_orderedVehiclesRepository = new Repository<OrderedVehicles>(OrderedVehicles.GetTableName()));
        }

        private IRepository<TrimEquipment> _trimEquipmentRepository;

        public IRepository<TrimEquipment> TrimEquipmentRepository
        {
            get => _trimEquipmentRepository ?? (_trimEquipmentRepository = new Repository<TrimEquipment>(TrimEquipment.GetTableName()));
        }

        private IRepository<Trim> _trimRepository;

        public IRepository<Trim> TrimRepository
        {
            get => _trimRepository ?? (_trimRepository = new Repository<Trim>(Trim.GetTableName()));
        }

        private IOrderRepository _orderRepository;

        public IOrderRepository OrderRepository
        {
            get => _orderRepository ?? (_orderRepository = new OrderRepository(Order.GetTableName()));
        }

        private IRepository<Model> _modelRepository;

        public IRepository<Model> ModelRepository
        {
            get => _modelRepository ?? (_modelRepository = new Repository<Model>(Model.GetTableName()));
        }

        private IRepository<Invoice> _invoiceRepository;

        public IRepository<Invoice> InvoiceRepository
        {
            get => _invoiceRepository ?? (_invoiceRepository = new Repository<Invoice>(Invoice.GetTableName()));
        }

        private IRepository<Employee> _employeeRepository;

        public IRepository<Employee> EmployeeRepository
        {
            get => _employeeRepository ?? (_employeeRepository = new Repository<Employee>(Employee.GetTableName()));
        }

        private IRepository<Engine> _engineRepository;

        public IRepository<Engine> EngineRepository
        {
            get => _engineRepository ?? (_engineRepository = new Repository<Engine>(Engine.GetTableName()));
        }

        private IRepository<Customer> _customerRepository;

        public IRepository<Customer> CustomerRepository
        {
            get => _customerRepository ?? (_customerRepository = new Repository<Customer>(Customer.GetTableName()));
        }

        public IEnumerable<Order> GetOrders(string selectedBranch, string selectedModel, string selectedEngine, string selectedTrim)
        {
            var taskOfBranches = Task.Factory.StartNew(()=> BranchRepository.GetAll());
            var taskOfEngines = Task.Factory.StartNew(() => EngineRepository.GetAll());
            var taskOfCustomers = Task.Factory.StartNew(() => CustomerRepository.GetAll());
            var taskOfEmployees = Task.Factory.StartNew(() => EmployeeRepository.GetAll());
            var taskOfInvoices = Task.Factory.StartNew(() => InvoiceRepository.GetAll());
            var taskOfModels = Task.Factory.StartNew(() => ModelRepository.GetAll());
            var taskOfModelEngines = Task.Factory.StartNew(() => ModelsEnginesIntersectRepository.GetAll());
            var taskOfOrders = Task.Factory.StartNew(() => OrderRepository.GetAll());
            var taskOfVehicles = Task.Factory.StartNew(() => OrderedVehiclesRepository.GetAll());
            var taskOfTrims = Task.Factory.StartNew(() => TrimRepository.GetAll());
            var taskOfTrimEquipments = Task.Factory.StartNew(() => TrimEquipmentRepository.GetAll());

            Task.WaitAll(taskOfTrimEquipments, taskOfTrims);

            var trims = taskOfTrims.Result;
            var trimEquipments = taskOfTrimEquipments.Result;
            
            foreach (var trim in trims)
            {
                trim.TrimEquipment = trimEquipments.Where(te => te.trim_name == trim.trim_name).ToList();
            }

            Task.WaitAll(taskOfEngines, taskOfModels, taskOfModelEngines);
            var vehicles = taskOfVehicles.Result;
            var engines = taskOfEngines.Result;
            var models = taskOfModels.Result;

            foreach (var vehicle in vehicles)
            {
                vehicle.Engine = engines.Single(e => e.engine_designation == vehicle.engine_designation);
                vehicle.Model = models.Single(m => m.model_name == vehicle.model_name);
                vehicle.Trim = trims.Single(t => t.trim_name == vehicle.trim_name);
            }

            Task.WaitAll(taskOfModelEngines);
            var modelEngines = taskOfModelEngines.Result;

            foreach (var modelEngine in modelEngines)
            {
                modelEngine.Engine = engines.Single(e => e.engine_designation == modelEngine.engine_designation);
                modelEngine.Model = models.Single(m => m.model_name == modelEngine.model_name);
            }

            Task.WaitAll(taskOfBranches, taskOfEmployees);
            var employees = taskOfEmployees.Result;
            var branches = taskOfBranches.Result;

            foreach (var employee in employees)
            {
                employee.Branch = branches.Single(b => b.branch_name == employee.branch_name);
            }

            Task.WaitAll(taskOfInvoices, taskOfOrders);
            var invoices = taskOfInvoices.Result;
            var orders = taskOfOrders.Result;

            foreach (var invoice in invoices)
            {
                invoice.Order = orders.Single(o => o.order_number == invoice.order_number);
            }

            Task.WaitAll(taskOfCustomers);
            var customers = taskOfCustomers.Result;

            foreach (var order in orders)
            {
                order.Customer = customers.Single(c => c.customer_number == order.customer_number);
                order.Employee = employees.Single(e => e.employee_number == order.employee_number);
                order.OrderedVehicles = vehicles.Where(v => v.order_number == order.order_number).ToList();
            }

            if (!string.IsNullOrEmpty(selectedBranch))
            {
                orders = orders.Where(o => o.Employee.branch_name == selectedBranch);
            }

            if (!string.IsNullOrEmpty(selectedEngine))
            {
                orders = orders.Where(o => o.OrderedVehicles.Any(v => v.engine_designation == selectedEngine));
            }

            if (!string.IsNullOrEmpty(selectedModel))
            {
                orders = orders.Where(o => o.OrderedVehicles.Any(v => v.model_name == selectedModel));
            }

            if (!string.IsNullOrEmpty(selectedTrim))
            {
                orders = orders.Where(o => o.OrderedVehicles.Any(v => v.Trim.trim_name == selectedTrim));
            }
            
            return orders.ToList();
        }

        public void Dispose()
        {
            if (_branchRepository != null) _branchRepository.Dispose();
            if (_engineRepository != null) _engineRepository.Dispose();
            if (_customerRepository != null) _customerRepository.Dispose();
            if (_employeeRepository != null) _employeeRepository.Dispose();
            if (_invoiceRepository != null) _invoiceRepository.Dispose();
            if (_modelRepository != null) _modelRepository.Dispose();
            if (_modelsEnginesIntersectRepository != null) _modelsEnginesIntersectRepository.Dispose();
            if (_orderRepository != null) _orderRepository.Dispose();
            if (_orderedVehiclesRepository != null) _orderedVehiclesRepository.Dispose();
            if (_trimRepository != null) _trimRepository.Dispose();
            if (_trimEquipmentRepository != null) _trimEquipmentRepository.Dispose();
        }
    }
}