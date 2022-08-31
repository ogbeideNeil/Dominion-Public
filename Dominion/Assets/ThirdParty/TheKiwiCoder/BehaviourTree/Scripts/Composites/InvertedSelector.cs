namespace TheKiwiCoder
{
    public class InvertedSelector : CompositeNode
    {
        protected int current;

        protected override void OnStart()
        {
            current = 0;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            for (int i = current; i < children.Count; ++i)
            {
                current = i;
                var child = children[current];

                switch (child.Update())
                {
                    case State.Running:
                        return State.Running;
                    case State.Failure:
                        return State.Success;
                    case State.Success:
                        continue;
                }
            }

            return State.Success;
        }
    }
}