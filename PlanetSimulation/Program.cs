using System;
using SFML.Graphics;
using SFML.Window;

namespace PlanetSimulation
{
    static class Program
    {
        public static RenderWindow Window;

        public static void Main(string[] args)
        {
            InitializeWindow();

            var simulation = new PlanetSimulator();

            while (Window.IsOpen())
            {
                Window.DispatchEvents();

                simulation.Update();

                Window.Clear(Color.Black);

                simulation.Render(Window);

                Window.Display();
            }
        }

        private static void InitializeWindow()
        {
            Window = new RenderWindow(new VideoMode(1600, 900, 32), "Planet Simulation", Styles.Default);
            //window.SetVerticalSyncEnabled(true);
            Window.SetActive(false);
            Window.SetVisible(true);

            Window.Closed += WindowClosedEvent;
        }

        private static void WindowClosedEvent(object sender, EventArgs e)
        {
            Window.Close();
        }
    }
}
