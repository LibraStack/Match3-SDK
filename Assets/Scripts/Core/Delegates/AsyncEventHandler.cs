using Cysharp.Threading.Tasks;

namespace Match3.Core.Delegates
{
    public delegate UniTask AsyncEventHandler<in TEventArgs>(object sender, TEventArgs e);
}