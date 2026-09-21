namespace CORE.Domain
{
    public abstract class Record
    {
        // Way 1:
        //private int id; // field

        //public void setId(int id) // behavior
        //{
        //    this.id = id;
        //}

        //public int getId()
        //{
        //    return id;
        //}

        // Way 2:
        public int Id { get; set; } // property

        protected Record()
        {
        }

        protected Record(int id)
        {
            Id = id;
        }
    }
}
