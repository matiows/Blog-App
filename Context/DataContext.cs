namespace Blog_App.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet <Blog> Blogs { get; set; }

        public DbSet <Tag> Tags { get; set; }

        public DbSet <UserAccount> UserAccounts { get; set; }

        public DbSet <BlogTag> BlogTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            //Set Composite keys
            modelBuilder.Entity<BlogTag>()
            .HasKey(o => new { o.BlogId, o.TagId });

            //Restrict Deletion
            modelBuilder.Entity<Blog>()
                .HasOne(t => t.UserAccount)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
