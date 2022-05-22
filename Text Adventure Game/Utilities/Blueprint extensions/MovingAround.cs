using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Text_Adventure_Game.Utilities.Blueprint_extensions
{
    public class MovingAround
    {
        private bool _isMoving;
        private Point? _buttonPosition;
        private double deltaX;
        private double deltaY;
        public TranslateTransform _currentTT;

        private Canvas Canvas;
        private Grid WholeBlueprint;

        private TransformGroup transformGroup = new TransformGroup();

        private UIElement controlToMOVE;

        public void MovingAroundCanvasInit(Canvas canvas, Grid canvasParent)
        {
            this.Canvas = canvas;
            WholeBlueprint = canvasParent;

            transformGroup.Children.Add(new MatrixTransform());
            transformGroup.Children.Add(new TranslateTransform());

            Canvas.RenderTransform = transformGroup;

            canvas.PreviewMouseLeftButtonDown += Canvas_PreviewMouseDown;
            canvas.PreviewMouseLeftButtonUp += Canvas_PreviewMouseUp;
            canvas.PreviewMouseMove += Canvas_PreviewMouseMove;
            canvas.MouseWheel += Canvas_MouseWheel;
        }

        public void MovingAroundFonctionInit(UIElement control)
        {
            controlToMOVE = control;
            controlToMOVE.MouseLeftButtonDown += new MouseButtonEventHandler(Control_MouseLeftButtonDown);
            controlToMOVE.MouseLeftButtonUp += new MouseButtonEventHandler(Control_MouseLeftButtonUp);
            controlToMOVE.MouseMove += new MouseEventHandler(Control_MouseMove);
        }

        protected bool isDragging;
        private Point clickPosition;

        private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(e.OriginalSource is Image))
            {
                isDragging = true;
                var draggableControl = sender as UserControl;
                clickPosition = e.GetPosition(controlToMOVE);
                draggableControl.CaptureMouse();
            }
            
        }

        private void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            var draggable = sender as UserControl;
            draggable.ReleaseMouseCapture();
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            var draggableControl = sender as UserControl;

            if (isDragging && draggableControl != null)
            {
                Point currentPosition = e.GetPosition((controlToMOVE as UserControl).Parent as UIElement);

                var transform = draggableControl.RenderTransform as TranslateTransform;
                if (transform == null)
                {
                    transform = new TranslateTransform();
                    draggableControl.RenderTransform = transform;
                }

                if (Canvas.GetLeft(controlToMOVE) >= 0 && Canvas.GetLeft(controlToMOVE) <= 5000 - (controlToMOVE as UserControl).Width)
                    Canvas.SetLeft(controlToMOVE, currentPosition.X - clickPosition.X);
                else
                {
                    if (Canvas.GetLeft(controlToMOVE) < 0)
                        Canvas.SetLeft(controlToMOVE, 0);
                    else
                        Canvas.SetLeft(controlToMOVE, 5000 - (controlToMOVE as UserControl).Width); 
                    
                    isDragging = false;
                }


                if (Canvas.GetTop(controlToMOVE) >= 0 && Canvas.GetTop(controlToMOVE) <= 5000 - (controlToMOVE as UserControl).Height)
                    Canvas.SetTop(controlToMOVE, currentPosition.Y - clickPosition.Y);

                else
                {
                    if(Canvas.GetTop(controlToMOVE) < 0)
                        Canvas.SetTop(controlToMOVE, 0);
                    else
                        Canvas.SetTop(controlToMOVE, 5000 - (controlToMOVE as UserControl).Height);

                    isDragging = false;
                }


            }
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var position = e.GetPosition(Canvas);
            var transform = (Canvas.RenderTransform as TransformGroup).Children[0] as MatrixTransform;
            var matrix = transform.Matrix;
            var scale = e.Delta >= 0 ? 1.1 : (1.0 / 1.1); // choose appropriate scaling factor

            matrix.ScaleAtPrepend(scale, scale, position.X, position.Y);
            (Canvas.RenderTransform as TransformGroup).Children[0] = new MatrixTransform(matrix);
        }

        private void Canvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Canvas)
            {
                if (_buttonPosition == null)
                    _buttonPosition = Canvas.TransformToAncestor(WholeBlueprint).Transform(new Point(0, 0));
                var mousePosition = Mouse.GetPosition(WholeBlueprint);
                deltaX = mousePosition.X - _buttonPosition.Value.X;
                deltaY = mousePosition.Y - _buttonPosition.Value.Y;
                _isMoving = true;
            }
        }

        private void Canvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            _currentTT = (Canvas.RenderTransform as TransformGroup).Children[1] as TranslateTransform;
            _isMoving = false;
        }

        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isMoving) return;

            var mousePoint = Mouse.GetPosition(WholeBlueprint);

            var offsetX = (_currentTT == null ? _buttonPosition.Value.X : _buttonPosition.Value.X - _currentTT.X) + deltaX - mousePoint.X;
            var offsetY = (_currentTT == null ? _buttonPosition.Value.Y : _buttonPosition.Value.Y - _currentTT.Y) + deltaY - mousePoint.Y;

            (this.Canvas.RenderTransform as TransformGroup).Children[1] = new TranslateTransform(-offsetX, -offsetY) ;
        }
    }
}
