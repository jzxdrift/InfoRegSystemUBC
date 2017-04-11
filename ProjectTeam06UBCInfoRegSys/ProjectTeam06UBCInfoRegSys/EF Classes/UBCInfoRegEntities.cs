using ProjectTeam06UBCInfoRegSys.EF_Classes;    //allows creating DbSets

namespace ProjectTeam06UBCInfoRegSys
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class UBCInfoRegEntities : DbContext
    {
        public UBCInfoRegEntities()
            : base("name=UBCInfoRegConnection") { } //connection string

        //DbSets representing database tables
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Registration> Registrations { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //disable default cascading on delete
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            /*
             * custom cascading on delete
             */
            modelBuilder.Entity<Course>()
                .HasMany(s => s.Sections)
                .WithRequired(s => s.Course)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Section>()
                .HasMany(s => s.Registrations)
                .WithRequired(s => s.Section)
                .WillCascadeOnDelete(true);
        }
    }
}