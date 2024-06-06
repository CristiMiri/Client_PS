using System;
using System.Collections.Generic;

namespace Client.Presenter
{
    class Subject
    {
        // Singleton instance
        private static Subject _instance;

        // Private constructor to prevent instantiation
        private Subject() { }

        // GetInstance method to retrieve the singleton instance
        public static Subject GetInstance()
        {
            // If the instance hasn't been created yet, create it
            if (_instance == null)
            {
                _instance = new Subject();
            }
            return _instance;
        }

        private readonly List<IObserver> _observers = new List<IObserver>();

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
    }
}
