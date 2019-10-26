using System.Threading.Tasks;

namespace RubberDuckPub
{
    public class Guest
    {
        public string Name { get; set; }

        public Guest(string name, Bar bar, MainWindow mainWindow)
        {
            Name = name;
            Task.Run(() =>
            {
                while (bar.IsOpen)
                {
                    if (bar.guestWaitingForTableQueue.Count > 0)
                    {
                        // here is where I am left 

                    }
                }
                //GoHome() after all lists with guests are empty 

            });

        }
    }
}
