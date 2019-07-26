namespace Kit.CoreV1
{
	// generated with SelectOverloadMultiple.js
    public partial class Select<T>
    {
        public void Enter<TLayer1, TLayer2>(T item) 
        {
            GetLayer<TLayer1>().Enter(item);
            GetLayer<TLayer2>().Enter(item);
        }
        public void Enter<TLayer1, TLayer2, TLayer3>(T item) 
        {
            GetLayer<TLayer1>().Enter(item);
            GetLayer<TLayer2>().Enter(item);
            GetLayer<TLayer3>().Enter(item);
        }
        public void Enter<TLayer1, TLayer2, TLayer3, TLayer4>(T item) 
        {
            GetLayer<TLayer1>().Enter(item);
            GetLayer<TLayer2>().Enter(item);
            GetLayer<TLayer3>().Enter(item);
            GetLayer<TLayer4>().Enter(item);
        }

        public void Exit<TLayer1, TLayer2>(T item) 
        {
            GetLayer<TLayer1>().Exit(item);
            GetLayer<TLayer2>().Exit(item);
        }
        public void Exit<TLayer1, TLayer2, TLayer3>(T item) 
        {
            GetLayer<TLayer1>().Exit(item);
            GetLayer<TLayer2>().Exit(item);
            GetLayer<TLayer3>().Exit(item);
        }
        public void Exit<TLayer1, TLayer2, TLayer3, TLayer4>(T item) 
        {
            GetLayer<TLayer1>().Exit(item);
            GetLayer<TLayer2>().Exit(item);
            GetLayer<TLayer3>().Exit(item);
            GetLayer<TLayer4>().Exit(item);
        }
    }
}