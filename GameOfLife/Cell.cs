using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameOfLife
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:GameOfLife"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:GameOfLife;assembly=GameOfLife"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:Cell/>
    ///
    /// </summary>
    public class Cell : Button
    {
        public Cell()
        {
            Click += Cell_Click;
            UpdateColor();
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            State = State.Equals(StateOfLife.Alive)?StateOfLife.Dead:StateOfLife.Alive;

            UpdateColor();
        }

        static Cell()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Cell), new FrameworkPropertyMetadata(typeof(Cell)));
        }

        private Cell[]Neighbours = new Cell[8];

        public Dictionary<StateOfLife, int> RenderNeighbours()
        {
            var toRet = new Dictionary<StateOfLife, int>();
            foreach (var neighbour in Neighbours)
            {
                if (!toRet.ContainsKey(neighbour.State))
                {
                    toRet.Add(neighbour.State, 1);
                    continue;
                }

                toRet[neighbour.State]++;
            }

            return toRet;
        }

        public StateOfLife State{get;set;}

        private StateOfLife Nextstate;

        public void SetNeighbours(Cell[]neighbours)
        {
            for (int i = 0; i < 8; i++)
            {
                Neighbours[i] = neighbours[i];
            }
        }

        public void Update()
        {
            var neighbours = RenderNeighbours();
            if (State.Equals(StateOfLife.Alive))
            {
                if (!neighbours.ContainsKey(StateOfLife.Alive) || neighbours[StateOfLife.Alive] == 1)
                {
                    Nextstate = StateOfLife.Dead;
                }
                else if (neighbours[StateOfLife.Alive] > 3)
                {
                    Nextstate = StateOfLife.Dead;
                }
                else
                {
                    Nextstate = StateOfLife.Alive;
                }
            }
            else if(neighbours.ContainsKey(StateOfLife.Alive) && neighbours[StateOfLife.Alive]==3)
            {
                Nextstate = StateOfLife.Alive;
            }
        }

        public void SetState(StateOfLife state)
        {
            State = state;
            UpdateColor();
        }

        public void ApplyUpdate()
        {
            State = Nextstate;
            UpdateColor();
        }

        private void UpdateColor()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Background = State.Equals(StateOfLife.Alive)
                    ? new SolidColorBrush(Color.FromRgb(0, (byte) 255, 0))
                    : new SolidColorBrush(Color.FromRgb(0, 0, 0));
            });
        }
    }

    public enum StateOfLife
    {
        Dead,
        Alive
    }
}
