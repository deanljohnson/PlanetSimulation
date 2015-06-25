using System;
using SFML.Graphics;
using SFML.Window;

namespace PlanetSimulation
{
    class Planet : Transformable, Drawable
    {
        private const bool m_ShowingVectors = true;
        private Vector2f m_forceThisFrame = new Vector2f(0, 0);
        private CircleShape m_shape;
        private RectangleShape m_velDisplay;

        public double Mass { get; private set; }

        public Vector2f ForceThisFrame
        {
            get { return m_forceThisFrame; }
            set { m_forceThisFrame = value; }
        }
        
        public Vector2f Velocity { get; set; }

        public Planet(double mass, Vector2f vel)
        {
            Mass = 0;
            Mass = mass;
            Velocity = vel;
            m_shape = null;
            m_velDisplay = null;

            SetDisplayTraits();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            m_shape.Position = Position;

            target.Draw(m_shape);

            if (m_ShowingVectors)
            {
                UpdateVectorTraits();
                target.Draw(m_velDisplay);
            }

            m_forceThisFrame = new Vector2f(0, 0);
        }

        private void SetDisplayTraits()
        {
            m_shape = new CircleShape((float)Math.Log(Mass) * 2f);
            m_shape.Origin = new Vector2f(m_shape.Radius / 2f, m_shape.Radius / 2f);
            m_shape.FillColor = Color.White;
            m_shape.Position = Position;
        }

        private void UpdateVectorTraits()
        {
            m_velDisplay = new RectangleShape(new Vector2f((float) Utils.Length(Velocity) * 5f, 2f))
            {
                Rotation = (float) (Math.Atan2(Velocity.Y, Velocity.X) * 180 / Math.PI),
                Origin = new Vector2f(0, 1),
                Position = m_shape.Position,
                FillColor = new Color(255, 0, 0, 175)
            };
        }
    }
}
