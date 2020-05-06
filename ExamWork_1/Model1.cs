namespace ExamWork_1
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    public class DbCreate : CreateDatabaseIfNotExists<Model1>
    {
        protected override void Seed(Model1 context)
        {
            base.Seed(context);
        }
    }
    public class Model1 : DbContext
    {

        public Model1()
            : base("name=Model1")
        {
            Database.SetInitializer(new DbCreate());
        }
        public virtual DbSet<TableExchangeRate> TableExchangeRate { get; set; }
    }

}