using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RPGProject.LivingEntity.EnemyStrategy;
using RPGProject.SubscriptionService;
using WindowsFormsApp1.GameEngine;

namespace WindowsFormsApp1.EnemyStrategy
{
    public abstract class MapStrategy : IMapStrategy, IGameObject
    {
        protected double DamageRadius;
        protected (double, double) TargetPositionOnMap;
        protected (double, double) CurrentPositionOnMap;
        protected double Speed;
        protected NameStrategy NameStrategy;
        public Guid Id;
        private bool _isActivated;

        protected abstract void Action();
        
        private void InvokeAction()
        {
            NavigateToTarget();
            if(!IsTargetReached() || !_isActivated) return;
            Action();
            Map.Map.Instance.RemoveSubscription(TypeSubscription.MapStrategy, Id);
            _isActivated = false;
        }
        
        public void ExecuteStrategy((double, double) startPoint, (double, double) endPoint)
        {
            _isActivated = true;
            CurrentPositionOnMap = startPoint;
            TargetPositionOnMap = endPoint;
            Id = Map.Map.Instance.AddSubscription(TypeSubscription.MapStrategy, InvokeAction, this);
        }

        private void NavigateToTarget()
        {
            var (px, py) = TargetPositionOnMap;
            var (ex, ey) = CurrentPositionOnMap;
            var dx = px - ex;
            var dy = py - ey;

            var angle = Math.Atan2(dy, dx);

            dx = Math.Cos(angle) * Speed;
            dy = Math.Sin(angle) * Speed;

            Move((dx, dy));
        }
        
        private bool IsTargetReached()
        {
            var (px, py) = TargetPositionOnMap;
            var (ex, ey) = CurrentPositionOnMap;
            var dx = px - ex;
            var dy = py - ey;

            var distance = Math.Sqrt(dx * dx + dy * dy);
            return distance <= DamageRadius;
        }
        
        private void Move((double, double) point)
        {
            var newX = CurrentPositionOnMap.Item1 + point.Item1;
            var newY = CurrentPositionOnMap.Item2 + point.Item2;
            ChangeCurrentPositionOnMap((newX, newY));
        }

        public void ChangeTargetPositionOnMap((double, double) point) =>
            TargetPositionOnMap = point;
        
        private void ChangeCurrentPositionOnMap((double, double) point) =>
            CurrentPositionOnMap = point;
        
        public void Draw(Graphics g)
        {
            g.DrawImage(
                Resources.GetFrame(NameStrategy.ToString()),
                (int)Math.Round(CurrentPositionOnMap.Item1),
                (int)Math.Round(CurrentPositionOnMap.Item2),
                50,
                50
            );
        }
    }
}