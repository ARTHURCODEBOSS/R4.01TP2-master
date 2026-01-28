using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP2P1.Controllers;
using TP2P1.Models.EntityFramework;

namespace TP2P1.Controllers.Tests
{
    [TestClass()]
    public class SeriesControllerTests
    {
        [TestMethod()]
        public void GetSeriesTest()
        {
            var builder = new DbContextOptionsBuilder<SeriesDbContext>().UseNpgsql("Server=localhost;port=5432;Database=SeriesDB; uid=postgres;  password=postgres;"); // Chaine de connexion à mettre dans les ( )
            SeriesDbContext context = new SeriesDbContext(builder.Options);
            SeriesController controller = new SeriesController(context);
            var listeSeriesRecuperees = controller.GetSeries().Result;
            List<Serie> listFinal =  listeSeriesRecuperees.Value.Where(s => s.Serieid <= 3).ToList();
            CollectionAssert.AreEqual(new List<Serie> { new Serie(1, "Scrubs", "J.D. est un jeune médecin qui débute sa carrière dans l'hôpital du Sacré-Coeur. Il vit avec son meilleur ami Turk, qui lui est chirurgien dans le même hôpital. Très vite, Turk tombe amoureux d'une infirmière Carla. Elliot entre dans la bande. C'est une étudiante en médecine quelque peu surprenante. Le service de médecine est dirigé par l'excentrique Docteur Cox alors que l'hôpital est géré par le diabolique Docteur Kelso. A cela viennent s'ajouter plein de personnages hors du commun : Todd le chirurgien obsédé, Ted l'avocat dépressif, le concierge qui trouve toujours un moyen d'embêter JD... Une belle galerie de personnage !", 9, 184, 2001, "ABC (US)"), new Serie(2, "James May's 20th Century", "The world in 1999 would have been unrecognisable to anyone from 1900. James May takes a look at some of the greatest developments of the 20th century, and reveals how they shaped the times we live in now.", 1, 6, 2007, "BBC Two"), new Serie(3, "True Blood", "Ayant trouvé un substitut pour se nourrir sans tuer (du sang synthétique), les vampires vivent désormais parmi les humains. Sookie, une serveuse capable de lire dans les esprits, tombe sous le charme de Bill, un mystérieux vampire. Une rencontre qui bouleverse la vie de la jeune femme...", 7, 81, 2008, "HBO") }, listFinal);

        }

        [TestMethod()]
        public void GetSerieTestTrue()
        {
            var builder = new DbContextOptionsBuilder<SeriesDbContext>().UseNpgsql("Server=localhost;port=5432;Database=SeriesDB; uid=postgres;  password=postgres;"); // Chaine de connexion à mettre dans les ( )
            SeriesDbContext context = new SeriesDbContext(builder.Options);
            SeriesController controller = new SeriesController(context);
            var Serie = controller.GetSerie(1).Result.Value;
            Assert.AreEqual(Serie, new Serie(1, "Scrubs", "J.D. est un jeune médecin qui débute sa carrière dans l'hôpital du Sacré-Coeur. Il vit avec son meilleur ami Turk, qui lui est chirurgien dans le même hôpital. Très vite, Turk tombe amoureux d'une infirmière Carla. Elliot entre dans la bande. C'est une étudiante en médecine quelque peu surprenante. Le service de médecine est dirigé par l'excentrique Docteur Cox alors que l'hôpital est géré par le diabolique Docteur Kelso. A cela viennent s'ajouter plein de personnages hors du commun : Todd le chirurgien obsédé, Ted l'avocat dépressif, le concierge qui trouve toujours un moyen d'embêter JD... Une belle galerie de personnage !", 9, 184, 2001, "ABC (US)"));
        }

        [TestMethod()]
        public void GetSerieTestFalse()
        {
            var builder = new DbContextOptionsBuilder<SeriesDbContext>().UseNpgsql("Server=localhost;port=5432;Database=SeriesDB; uid=postgres;  password=postgres;"); // Chaine de connexion à mettre dans les ( )
            SeriesDbContext context = new SeriesDbContext(builder.Options);
            SeriesController controller = new SeriesController(context);
            var Serie = controller.GetSerie(0).Result;
            Assert.AreEqual(StatusCodes.Status404NotFound,((NotFoundResult)Serie.Result).StatusCode);
        }
        
        [TestMethod()]
        public void DeleteSerieTestTrue()
        {
            var builder = new DbContextOptionsBuilder<SeriesDbContext>().UseNpgsql("Server=localhost;port=5432;Database=SeriesDB; uid=postgres;  password=postgres;"); // Chaine de connexion à mettre dans les ( )
            SeriesDbContext context = new SeriesDbContext(builder.Options);
            SeriesController controller = new SeriesController(context);
            var delete = controller.DeleteSerie(59).Result;
            
            Assert.AreEqual(204, ((NoContentResult)delete).StatusCode);
        }

        [TestMethod()]
        public void DeleteSerieTestFalse()
        {
            var builder = new DbContextOptionsBuilder<SeriesDbContext>().UseNpgsql("Server=localhost;port=5432;Database=SeriesDB; uid=postgres;  password=postgres;"); // Chaine de connexion à mettre dans les ( )
            SeriesDbContext context = new SeriesDbContext(builder.Options);
            SeriesController controller = new SeriesController(context);
            var Serie = controller.DeleteSerie(0).Result;
            Assert.AreEqual(StatusCodes.Status404NotFound, ((NotFoundResult)Serie).StatusCode);
        }
        [TestMethod()]
        public void PutSerieTestFalse()
        {
            var builder = new DbContextOptionsBuilder<SeriesDbContext>().UseNpgsql("Server=localhost;port=5432;Database=SeriesDB; uid=postgres;  password=postgres;"); // Chaine de connexion à mettre dans les ( )
            SeriesDbContext context = new SeriesDbContext(builder.Options);
            SeriesController controller = new SeriesController(context);
            var Serie = controller.PutSerie(138,new Serie(138,"TitreTest","ResumerTest",138,138,2138,"NetTes")).Result;
            Assert.AreEqual(controller.GetSerie(138).Result.Value, new Serie(138, "TitreTest", "ResumerTest", 138, 138, 2138, "NetTes"));
        }
        
        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void PostSerieTest_NotOK()
        {
            var builder = new DbContextOptionsBuilder<SeriesDbContext>()
                .UseNpgsql("Server=localhost;port=5432;Database=SeriesDB; uid=postgres; password=postgres;");
            SeriesDbContext context = new SeriesDbContext(builder.Options);
            SeriesController controller = new SeriesController(context);
            Serie serieInvalide = new Serie(139, null, "Ca va planter", 1, 1, 2024, "CrashTV");
            var result = controller.PostSerie(serieInvalide).Result;
        }



    }
}