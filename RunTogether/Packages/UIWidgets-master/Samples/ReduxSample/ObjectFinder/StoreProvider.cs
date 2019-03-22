namespace Unity.UIWidgets.Sample.Redux.ObjectFinder {
    public static class StoreProvider {
        static Store<FinderAppState> _store;

        public static Store<FinderAppState> store {
            get {
                if (_store != null) {
                    return _store;
                }

                var middlewares = new Middleware<FinderAppState>[] {
                    ReduxLogging.Create<FinderAppState>(),
                    GameFinderMiddleware.Create(),
                };
                _store = new Store<FinderAppState>(ObjectFinderReducer.Reduce,
                    new FinderAppState(),
                    middlewares
                );
                return _store;
            }
        }
    }
}