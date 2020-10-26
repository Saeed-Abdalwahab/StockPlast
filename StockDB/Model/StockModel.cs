namespace StockDB.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class StockModel : DbContext
    {
        public StockModel()
            : base("name=StockModel")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetModule> AspNetModules { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserType> AspNetUserTypes { get; set; }
        public virtual DbSet<AspNetUserWorker> AspNetUserWorkers { get; set; }
        public virtual DbSet<color> colors { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
 
        public virtual DbSet<DemondOrder> DemondOrders { get; set; }
        public virtual DbSet<DemondOrderDetail> DemondOrderDetails { get; set; }
        public virtual DbSet<Employe> Employes { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<ExpenseDetail> ExpenseDetails { get; set; }
        public virtual DbSet<FromStock> FromStocks { get; set; }
        public virtual DbSet<HandType> HandTypes { get; set; }
        public virtual DbSet<Income> Incomes { get; set; }
        public virtual DbSet<IncomeDetail> IncomeDetails { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemSupplier> ItemSuppliers { get; set; }
        public virtual DbSet<JobDescription> JobDescriptions { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Material_Type> Material_Type { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }

        public virtual DbSet<Safe> Safes { get; set; }
        public virtual DbSet<SafeTransaction> SafeTransactions { get; set; }


        public virtual DbSet<Shaplona> Shaplonas { get; set; }
        public virtual DbSet<StockTransaction> StockTransactions { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<StoreType> StoreTypes { get; set; }
        public virtual DbSet<supplier> suppliers { get; set; }
       
        public virtual DbSet<ToStock> ToStocks { get; set; }
        public virtual DbSet<CustomerPayment> CustomerPayments { get; set; }
        public virtual DbSet<SupplierPayments> SupplierPayments { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetModule>()
                .HasMany(e => e.AspNetRoles)
                .WithRequired(e => e.AspNetModule)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<Item>()
           .HasMany(e => e.DemondOrderDetails)
           .WithOptional(e => e.Item)
           .HasForeignKey(e => e.Item_id);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserWorkers)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUserType>()
                .HasMany(e => e.AspNetUserWorkers)
                .WithRequired(e => e.AspNetUserType)
                .HasForeignKey(e => e.UserTypeId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Bank>()
               .HasMany(e => e.CustomerPayments)
               .WithOptional(e => e.Bank)
               .HasForeignKey(e => e.Bank_ID);
            modelBuilder.Entity<Bank>()
    .HasMany(e => e.SupplierPayments)
    .WithOptional(e => e.Bank)
    .HasForeignKey(e => e.Bank_ID);
            modelBuilder.Entity<color>()
                .HasMany(e => e.Items)
                .WithOptional(e => e.color)
                .HasForeignKey(e => e.ColorID);
            modelBuilder.Entity<Customer>()
                .HasMany(e => e.CustomerPayments)
                .WithOptional(e => e.Customer)
                .HasForeignKey(e => e.Customer_ID);
            modelBuilder.Entity<supplier>()
              .HasMany(e => e.SupplierPayments)
              .WithOptional(e => e.Supplier)
              .HasForeignKey(e => e.Supplier_ID);
            modelBuilder.Entity<Customer>()
                .HasMany(e => e.DemondOrders)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<color>()
                  .HasMany(e => e.DemondOrderDetails)
                  .WithOptional(e => e.color)
                  .HasForeignKey(e => e.HandColorID);


            modelBuilder.Entity<DemondOrder>()
                .HasMany(e => e.FromStocks)
                .WithOptional(e => e.DemondOrder)
                .HasForeignKey(e => e.DemondOrderId);

           

            modelBuilder.Entity<DemondOrderDetail>()
                .HasMany(e => e.StockTransactions)
                .WithOptional(e => e.DemondOrderDetail)
                .HasForeignKey(e => e.DemondOrderDetialsId);

            modelBuilder.Entity<DemondOrderDetail>()
                .HasMany(e => e.ToStocks)
                .WithOptional(e => e.DemondOrderDetail)
                .HasForeignKey(e => e.DemondOrderDetialsId);
            modelBuilder.Entity<Safe>()
             .HasMany(e => e.SafeTransactions)
             .WithOptional(e => e.Safe)
             .HasForeignKey(e => e.SafeID);

            modelBuilder.Entity<Employe>()
        .HasMany(e => e.FromStocks)
        .WithRequired(e => e.Employe)
        .HasForeignKey(e => e.CutTechEmp_ID);

            modelBuilder.Entity<Employe>()
    .HasMany(e => e.FromStocks1)
    .WithRequired(e => e.Employe1)
    .HasForeignKey(e => e.PrintTechEmp_ID);

     

            modelBuilder.Entity<FromStock>()
               .HasMany(e => e.ToStocks)
               .WithOptional(e => e.FromStock)
               .HasForeignKey(e => e.FromStockId);

            modelBuilder.Entity<HandType>()
                .HasMany(e => e.DemondOrderDetails)
                .WithOptional(e => e.HandType)
                .HasForeignKey(e => e.HandTypeId);

            modelBuilder.Entity<Income>()
                .HasMany(e => e.IncomeDetails)
                .WithOptional(e => e.Income)
                .HasForeignKey(e => e.Income_inc_id);

            modelBuilder.Entity<JobDescription>()
                .HasMany(e => e.Employes)
                .WithRequired(e => e.JobDescription)
                .HasForeignKey(e => e.jobDesc_id);


            modelBuilder.Entity<Job>()
                .HasMany(e => e.JobDescriptions)
                .WithRequired(e => e.Job)
                .HasForeignKey(e => e.JobID)
                .WillCascadeOnDelete(false);



            modelBuilder.Entity<Material_Type>()
                .HasMany(e => e.Items)
                .WithOptional(e => e.Material_Type)
                .HasForeignKey(e => e.MatTypeId);

            modelBuilder.Entity<PaymentType>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<PaymentType>()
              .HasMany(e => e.CustomerPayments)
              .WithOptional(e => e.PaymentType)
              .HasForeignKey(e => e.PaymentTyp_ID);
            modelBuilder.Entity<PaymentType>()
           .HasMany(e => e.SupplierPayments)
           .WithOptional(e => e.PaymentType)
           .HasForeignKey(e => e.PaymentTyp_ID);


            modelBuilder.Entity<DemondOrder>()
     .HasMany(e => e.SafeTransactions)
     .WithOptional(e => e.DemondOrder)
     .HasForeignKey(e => e.DemondorderID);


            modelBuilder.Entity<ExpenseDetail>()
              .HasMany(e => e.SafeTransactions)
              .WithOptional(e => e.ExpenseDetail)
              .HasForeignKey(e => e.ExpenseDetailsID);

            modelBuilder.Entity<IncomeDetail>()
         .HasMany(e => e.SafeTransactions)
         .WithOptional(e => e.IncomeDetail)
         .HasForeignKey(e => e.IncomeDetailsID);


           

            modelBuilder.Entity<Safe>()
                .HasMany(e => e.DemondOrders)
                .WithRequired(e => e.Safe)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<Shaplona>()
                .HasMany(e => e.DemondOrderDetails)
                .WithRequired(e => e.Shaplona)
                .WillCascadeOnDelete(false);

        

            modelBuilder.Entity<Store>()
                .HasMany(e => e.StockTransactions)
                .WithOptional(e => e.Store)
                .HasForeignKey(e => e.StoreId);

            modelBuilder.Entity<StoreType>()
                .HasMany(e => e.Stores)
                .WithRequired(e => e.StoreType)
                .HasForeignKey(e => e.StoreType_ID);

            modelBuilder.Entity<supplier>()
                .HasMany(e => e.ItemSuppliers)
                .WithOptional(e => e.supplier)
                .HasForeignKey(e => e.SupId);

            modelBuilder.Entity<supplier>()
                .HasMany(e => e.ToStocks)
                .WithOptional(e => e.supplier)
                .HasForeignKey(e => e.SupId);


            modelBuilder.Entity<ToStock>()
                .HasMany(e => e.FromStocks)
                .WithOptional(e => e.ToStock)
                .HasForeignKey(e => e.ToStockId);

        
    
        }


    }
}
