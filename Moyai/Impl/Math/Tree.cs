namespace Moyai.Impl.Math
{
	public class TreeNode<T>
	{
		public T Value { get; set; }
		public TreeNode<T>? Parent { get; protected set; }
		public List<TreeNode<T>> Children { get; set; }

		public TreeNode(T value)
		{
			Value = value;
			Children = [];
		}

		public static explicit operator T(TreeNode<T> node) => node.Value;
		public static explicit operator TreeNode<T>(T v) => new(v);

		public void Traverse(Action<T> action)
		{
			action(Value);
			foreach(var child in Children) child.Traverse(action);
		}

		public TreeNode<T> AddChild(T child)
		{
			TreeNode<T> node = new(child) { Parent = this };
			Children.Add(node);
			return node;
		}
		public void RemoveChild(TreeNode<T> child)
		{
			child.Parent = null;
			Children.Remove(child);
		}
		public void SetParent(TreeNode<T> parent)
		{
			Parent?.Children.Remove(this);
			Parent = parent;
			Parent.Children.Add(this);
		}
		public void RemoveParent()
		{
			if(Parent != null)
			{
				Parent.Children.Remove(this);
				Parent = null;
			}
		}
		public TreeNode<T>? FindChild(Func<TreeNode<T>, bool> predicate)
		{
			foreach(var node in Children)
				if(!predicate(node))
					node.FindChild(predicate);
				else
					return node;
			return null;
		}
	}
}
