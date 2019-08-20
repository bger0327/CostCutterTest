using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;
using ui.Models;
using ui.Repositories;

namespace ui
{
    public partial class Form1 : Form
    {
        private int _expandingRowIndex;
        private int _order;

        public Form1()
        {
            InitializeComponent();

            SetOrderGridFunctions();
            SetFilters();
            SetOrderGridTemplates();

            InitializeModel();

            GetBranches();
            GetEngines();
            GetModels();
            GetTrims();


            SearchButton.Click += SearchButton_Click;
        }

        private void SetOrderGridFunctions()
        {
            OrdersGrid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            OrdersGrid.AutoGenerateColumns = false;
            //OrdersGrid.DataSource = orderBindingSource;
            OrdersGrid.EnableFiltering = true;
            OrdersGrid.MasterTemplate.ShowHeaderCellButtons = true;
            OrdersGrid.MasterTemplate.ShowFilteringRow = false;
            OrdersGrid.EnablePaging = true;
            OrdersGrid.PageSize = 20;

            OrdersGrid.ChildViewExpanding += OrdersGridOnChildViewExpanding;
            OrdersGrid.ChildViewExpanded += OrdersGridOnChildViewExpanded;
            OrdersGrid.CellValueChanged += OrdersGridOnCellValueChanged;
        }

        private void OrdersGridOnCellValueChanged(object sender, GridViewCellEventArgs e)
        {
            var order = (Order) e.Row.DataBoundItem;

            using (var unitOfWork = new UnitOfWork())
            {
                unitOfWork.OrderRepository.Put(order);
            }
        }

        private void OrdersGridOnChildViewExpanding(object sender, ChildViewExpandingEventArgs e)
        {
            _order = _expandingRowIndex > e.ParentRow.Index ? 1 : 0;
            _expandingRowIndex = e.ParentRow.Index;
        }
        
        private void OrdersGridOnChildViewExpanded(object sender, ChildViewExpandedEventArgs e)
        {
            if (e.IsExpanded)
            {
                if (e.ParentRow.DataBoundItem is Order)
                {
                    var rows = OrdersGrid.MasterTemplate.Rows.ToList();

                    if (_order == 0) rows = rows.OrderByDescending(r => r.Index).ToList();

                    foreach (GridViewRowInfo row in rows)
                    {
                        if (row.Index != _expandingRowIndex)
                        {
                            row.IsExpanded = false;
                        }
                        else
                        {
                            row.IsExpanded = true;
                        }
                    }

                    var order = (Order) e.ParentRow.DataBoundItem;
                    e.ChildViewInfo.ViewTemplate.DataSource = order.OrderedVehicles;
                    e.ChildViewInfo.ViewTemplate.Refresh();
                }

                if (e.ParentRow.DataBoundItem is OrderedVehicles)
                {
                    var rows = OrdersGrid.Templates[0].Rows.ToList();

                    if (_order == 0) rows = rows.OrderByDescending(r => r.Index).ToList();

                    foreach (GridViewRowInfo row in rows)
                    {
                        if (row.Index != _expandingRowIndex)
                        {
                            row.IsExpanded = false;
                        }
                        else
                        {
                            row.IsExpanded = true;
                        }
                    }

                    var order = (OrderedVehicles)e.ParentRow.DataBoundItem;
                    e.ChildViewInfo.ViewTemplate.DataSource = order.Trim.TrimEquipment;
                    e.ChildViewInfo.ViewTemplate.Refresh();
                }
            }
        }

        private void SetFilters()
        {
            BranchList.DropDownListElement.AutoCompleteSuggest.SuggestMode = SuggestMode.Contains;
            ModelList.DropDownListElement.AutoCompleteSuggest.SuggestMode = SuggestMode.Contains;
            TrimEquipmentList.DropDownListElement.AutoCompleteSuggest.SuggestMode = SuggestMode.Contains;
            EngineList.DropDownListElement.AutoCompleteSuggest.SuggestMode = SuggestMode.Contains;
        }

        private void SetOrderGridTemplates()
        {
            GridViewTemplate template = CreateOrderDetailsTemplate();
            GridViewRelation relation = new GridViewRelation(OrdersGrid.MasterTemplate, template);
            relation.RelationName = "Vehicles";
            OrdersGrid.Relations.Add(relation);

        }

        private GridViewTemplate CreateOrderDetailsTemplate()
        {
            GridViewTemplate orderDetailsTemplate = new GridViewTemplate();
            OrdersGrid.Templates.Add(orderDetailsTemplate);
            GridViewTextBoxColumn orderColumn = new GridViewTextBoxColumn("vehicle_number");
            orderDetailsTemplate.Columns.Add(orderColumn);
            orderColumn = new GridViewTextBoxColumn("order_number");
            orderDetailsTemplate.Columns.Add(orderColumn);
            orderColumn = new GridViewTextBoxColumn("model_name");
            orderDetailsTemplate.Columns.Add(orderColumn);
            orderColumn = new GridViewTextBoxColumn("engine_designation");
            orderDetailsTemplate.Columns.Add(orderColumn);
            orderColumn = new GridViewTextBoxColumn("colour");
            orderDetailsTemplate.Columns.Add(orderColumn);
            orderColumn = new GridViewTextBoxColumn("expected_delivery_date");
            orderDetailsTemplate.Columns.Add(orderColumn);
            orderColumn = new GridViewTextBoxColumn("actual_delivery_date");
            orderDetailsTemplate.Columns.Add(orderColumn);
            orderColumn = new GridViewTextBoxColumn("Model.base_cost");
            orderDetailsTemplate.Columns.Add(orderColumn);
            orderColumn = new GridViewTextBoxColumn("Model.average_lead_time");
            orderDetailsTemplate.Columns.Add(orderColumn);
            orderColumn = new GridViewTextBoxColumn("Engine.fuel_type");
            orderDetailsTemplate.Columns.Add(orderColumn);
            orderColumn = new GridViewTextBoxColumn("Trim.additional_cost");
            orderDetailsTemplate.Columns.Add(orderColumn);
            orderDetailsTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            var trimDetailsTemplate = CreateTrimDetailsTemplate(orderDetailsTemplate);

            GridViewRelation relation = new GridViewRelation(orderDetailsTemplate, trimDetailsTemplate);
            relation.RelationName = "TrimEquipment";
            OrdersGrid.Relations.Add(relation);

            return orderDetailsTemplate;
        }

        private GridViewTemplate CreateTrimDetailsTemplate(GridViewTemplate orderDetailsTemplate)
        {
            GridViewTemplate trimDetailsTemplate = new GridViewTemplate();
            orderDetailsTemplate.Templates.Add(trimDetailsTemplate);
            GridViewTextBoxColumn trimColumn = new GridViewTextBoxColumn("trim_name");
            trimDetailsTemplate.Columns.Add(trimColumn);
            trimColumn = new GridViewTextBoxColumn("start_date");
            trimDetailsTemplate.Columns.Add(trimColumn);
            trimColumn = new GridViewTextBoxColumn("end_date");
            trimDetailsTemplate.Columns.Add(trimColumn);
            trimColumn = new GridViewTextBoxColumn("wheel_type");
            trimDetailsTemplate.Columns.Add(trimColumn);
            trimColumn = new GridViewTextBoxColumn("infotainment_type");
            trimDetailsTemplate.Columns.Add(trimColumn);
            trimColumn = new GridViewTextBoxColumn("headlight_type");
            trimDetailsTemplate.Columns.Add(trimColumn);
            trimColumn = new GridViewTextBoxColumn("upholstery_type");
            trimDetailsTemplate.Columns.Add(trimColumn);
            trimDetailsTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            return trimDetailsTemplate;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var task = Task.Factory.StartNew(() => unitOfWork.GetOrders(BranchList.SelectedItem?.Text, ModelList.SelectedItem?.Text, EngineList.SelectedItem?.Text, TrimEquipmentList.SelectedItem?.Text));
                    
                LoadingBar.StartWaiting();
                LoadingBar.AssociatedControl = OrdersGrid;
                LoadingBar.Visible = true;

                task.ContinueWith(t =>
                {
                    Invoke(new SetOrderDataSourceDelegate(SetOrderDataSource),t.Result);
                    Invoke(new StopLoadingBarDelegate(StopLoadingBar));
                });
            }

        }

        public delegate void SetOrderDataSourceDelegate(IEnumerable<Order> orders);
        public delegate void StopLoadingBarDelegate();

        public void SetOrderDataSource(IEnumerable<Order> orders)
        {
            OrdersGrid.DataSource = orders;

            foreach (var column in OrdersGrid.Columns)
            {
                column.BestFit();
            }
        }

        private void  StopLoadingBar()
        {
            LoadingBar.AssociatedControl = null;
            LoadingBar.Visible = false;
            LoadingBar.StopWaiting();
            LoadingBar.ResetWaiting();
        }

        private void InitializeModel()
        {
            Branches = new ObservableCollection<Branch>();
            Orders = new ObservableCollection<Order>();
            Models = new ObservableCollection<Model>();
            Engines = new ObservableCollection<Engine>();
            Trims = new ObservableCollection<Trim>();
        }

        private void GetBranches()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                if (!Branches.Any())
                {
                    Branches.Clear();
                    Branches.Add(new Branch(){branch_name = ""});
                    Branches.AddRange(unitOfWork.BranchRepository.GetAll());
                }

                branchBindingSource.DataSource = Branches;
            }
        }

        private void GetModels()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                if (!Models.Any())
                {
                    Models.Clear();
                    Models.Add(new Model(){model_name = ""});
                    Models.AddRange(unitOfWork.ModelRepository.GetAll());
                }

                modelBindingSource.DataSource = Models;
            }
        }

        private void GetEngines()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                if (!Engines.Any())
                {
                    Engines.Clear();
                    Engines.Add(new Engine(){engine_designation = ""});
                    Engines.AddRange(unitOfWork.EngineRepository.GetAll());
                }

                engineBindingSource.DataSource = Engines;
            }
        }

        private void GetTrims()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                if (!Trims.Any())
                {
                    Trims.Clear();
                    Trims.Add(new Trim(){trim_name = ""});
                    Trims.AddRange(unitOfWork.TrimRepository.GetAll());
                }

                trimEquipmentBindingSource.DataSource = Trims;
            }
        }

        public ObservableCollection<Branch> Branches { get; set; }
        public ObservableCollection<Model> Models { get; set; }
        public ObservableCollection<Engine> Engines { get; set; }
        public ObservableCollection<Trim> Trims { get; set; }
        public ObservableCollection<Order> Orders { get; set; }
    }
}