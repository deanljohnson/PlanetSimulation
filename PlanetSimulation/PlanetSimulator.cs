using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;

namespace PlanetSimulation
{
    class PlanetSimulator
    {
        private const double m_GravConst = .0005;
        private const double m_MaxPlanetMass = 100;
        private static readonly Random Random = new Random();
        private readonly List<Planet> m_planets = new List<Planet>();
        private const int m_NumPlanets = 300;

        public PlanetSimulator()
        {
            InitializePlanets();
        }

        public void Update()
        {
            UpdatePlanetVelocities();
            UpdatePlanetPositions();
        }

        public void Render(RenderWindow window)
        {
            CenterViewOnCenterOfMass();
            foreach (var i in m_planets)
            {
                window.Draw(i);
            }
        }

        private void InitializePlanets()
        {
            for (var i = 0; i < m_NumPlanets; i++)
            {
                //Add 1 to the random mass so we do not get a mass of 0
                m_planets.Add(new Planet((double)Random.Next((int)m_MaxPlanetMass) + 1, new Vector2f((float)Random.NextDouble() - (float)Random.NextDouble(), (float)Random.NextDouble() - (float)Random.NextDouble())));
                m_planets[i].Position = new Vector2f((float)(Random.NextDouble() * Program.Window.Size.X), (float)(Random.NextDouble() * Program.Window.Size.Y));
            }
        }

        /// <summary>
        /// Updates the velocity of all planets based on the force of gravity exerted between planets
        /// </summary>
        private void UpdatePlanetVelocities()
        {
            //foreach loop is not efficient in this case, we need to maintain knowledge about the index of a planet
            for (var i = 0; i < m_planets.Count; i++)
            {
                for (var j = 0; j < m_planets.Count; j++)
                {
                    //Since the gravitational force obeys Newton's Third Law, we don't want to recalculate a reaction we have already visited
                    if (j <= i)
                        continue;

                    var force = ForceBetweenPlanets(m_planets[i], m_planets[j]);
                    m_planets[i].ForceThisFrame += force; //Equal and opposite forces
                    m_planets[j].ForceThisFrame -= force; //Equal and opposite forces
                }

                m_planets[i].Velocity += (m_planets[i].ForceThisFrame / (float)m_planets[i].Mass);
            }
        }

        /// <summary>
        /// Updates the position of all planets with their current velocity
        /// </summary>
        private void UpdatePlanetPositions()
        {
            foreach (var i in m_planets)
            {
                i.Position += i.Velocity;
            }
        }

        /// <summary>
        /// Returns the gravitional force between A & B, in the direction of A to B
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static Vector2f ForceBetweenPlanets(Planet a, Planet b)
        {
            var d = Utils.Distance(a.Position, b.Position);
            var forceMag = m_GravConst * (a.Mass * b.Mass) / d; //Newton's Law of Universal Gravitation

            var forceVector = b.Position - a.Position;
            forceVector /= (float)d; //reduce to unit vector
            forceVector *= (float)forceMag; //scale to proper magnitude

            return forceVector;
        }

        /// <summary>
        /// Adjusts the viewport position to be centered on the center of mass
        /// </summary>
        private void CenterViewOnCenterOfMass()
        {
            var centerOfMass = CenterOfMass();
            var screenCenter = new Vector2f(Program.Window.Size.X / 2, Program.Window.Size.Y / 2);
            var difVector = screenCenter - centerOfMass;

            Program.Window.DefaultView.Move(difVector);
        }

        /// <summary>
        /// Calculates and return the center of mass of all the planets in the simulation
        /// </summary>
        /// <returns></returns>
        private Vector2f CenterOfMass()
        {
            var centerOfMass = new Vector2f(0, 0);
            double totalMass = 0;

            foreach (var i in m_planets)
            {
                totalMass += i.Mass;
                centerOfMass += ((float)i.Mass * i.Position);
            }

            centerOfMass /= (float)totalMass;

            return centerOfMass;
        }
    }
}
