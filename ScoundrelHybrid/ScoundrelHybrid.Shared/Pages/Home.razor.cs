using ScoundrelCore.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoundrelHybrid.Shared.Pages
{
    public partial class Home
    {
        public GameEngine gameEngine = new GameEngine();

        private string factor => FormFactor.GetFormFactor();
        private string platform => FormFactor.GetPlatform();
    }
}
