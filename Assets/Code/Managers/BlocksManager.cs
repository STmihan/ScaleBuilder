using System.Collections.Generic;
using System.Linq;

namespace Code.Managers
{
    public class BlocksManager : Singleton<BlocksManager>
    {
        private readonly List<Block> _blocks = new List<Block>();

        public void AddBlock(Block block)
        {
            _blocks.Add(block);
        }

        public float GetBlocksHeight()
        {
            return _blocks
                .Select(block => block.transform.position.y + block.Height)
                .Prepend(0)
                .Max();
        }
    }
}