using System;
using System.Collections.Generic;
using System.Linq;

namespace RPGProject.SubscriptionService
{
    public class SubscriptionService
    {
        private readonly Dictionary<TypeSubscription, List<(Guid, Action)>> _subscriptions = 
            new Dictionary<TypeSubscription, List<(Guid, Action)>>();

        public Guid Subscribe(TypeSubscription eventName, Action eventHandler)
        {
            if (!_subscriptions.ContainsKey(eventName))
            {
                _subscriptions[eventName] = new List<(Guid, Action)>();
            }
            
            Guid subscriptionId = GenerateSubscriptionId();
            _subscriptions[eventName].Add((subscriptionId, eventHandler));
            
            return subscriptionId;
        }

        public void Unsubscribe(TypeSubscription eventName, Guid subscriptionId)
        {
            if (_subscriptions.TryGetValue(eventName, out var subscriptions))
            {
                foreach (var subscription in subscriptions.ToList())
                {
                    if (subscription.Item1 == subscriptionId)
                    {
                        _subscriptions[eventName].Remove(subscription);
                    }
                }
            }
        }

        private Guid GenerateSubscriptionId()
        {
            return Guid.NewGuid();
        }

        public List<Action> GetSubscriptions(TypeSubscription eventName)
        {
            if (_subscriptions.TryGetValue(eventName, out var subscription))
            {
                return subscription.ConvertAll(s => s.Item2);
            }
            return new List<Action>();
        }
    }
}