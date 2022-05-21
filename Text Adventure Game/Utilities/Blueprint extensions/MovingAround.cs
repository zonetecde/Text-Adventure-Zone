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

        

        public void MovingAroundInit(Canvas canvas, Grid canvasParent)
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
            if (_buttonPosition == null)
                _buttonPosition = Canvas.TransformToAncestor(WholeBlueprint).Transform(new Point(0, 0));
            var mousePosition = Mouse.GetPosition(WholeBlueprint);
            deltaX = mousePosition.X - _buttonPosition.Value.X;
            deltaY = mousePosition.Y - _buttonPosition.Value.Y;
            _isMoving = true;
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
