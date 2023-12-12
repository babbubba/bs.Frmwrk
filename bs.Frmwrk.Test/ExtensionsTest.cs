using bs.Frmwrk.Auth.Services;
using bs.Frmwrk.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace bs.Frmwrk.Test
{
    public class ExtensionsTest
    {
        [Test]
        public async Task Except_Test()
        {
            var log = Root.ServiceProvider?.GetRequiredService<ILogger<ExtensionsTest>>();
            var list1 = new List<ListElement>
            {
                new ListElement("Pinco", "Pallino", 1),
                new ListElement("Mario", "Rossi", 2),
                new ListElement("Mario", "Verdi", 3),
                new ListElement("Gigi", "La Trottola", 4)
            };

            var list2 = new List<ListElement>
            {
                new ListElement("Mario", "Rossi", 2),
                new ListElement("Guido", "La Vespa", 5),
                new ListElement("Gustavo", "La Torta", 6),
            };

            var exceptList = list1.Except(list2, l => l.Id);

            Assert.That(exceptList.Any(el => el.Id == 2), Is.False, "Except extension doesnt work properly");
        }
    }

    internal class ListElement
    {
        public ListElement(string name, string lastName, int id)
        {
            Name = name;
            LastName = lastName;
            Id = id;
        }

        public string Name { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }
    }
}