using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TrueMarbleData
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class TMDataControllerImpl : ITMDataController
    {
        public TMDataControllerImpl()
        {
            //Console.WriteLine("Welcome to TrueMarble Data!\n");
            Console.WriteLine("New Client connected to the Server");
        }

        public int GetTileWidth()
        {
            int width, height;
            TMDLLWrapper.GetTileSize(out width, out height);
            return width;
        }

        public int GetTileHeight()
        {
            int width, height;
            TMDLLWrapper.GetTileSize(out width, out height);
            return height;
        }

        public int GetNumTilesAcross(int zoom)
        {
            int x, y;
            TMDLLWrapper.GetNumTiles(zoom, out x, out y);
            return x;
        }

        public int GetNumTilesDown(int zoom)
        {
            int x, y;
            TMDLLWrapper.GetNumTiles(zoom, out x, out y);
            return y;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public byte[] LoadTile(int zoom, int x, int y)
        {
            int iBufSize, iRetSize;
            byte[] newBuf; //Buffer to hold the raw JPEG data.
            iBufSize = GetTileWidth() * GetTileHeight() * 3;
            newBuf = new byte[iBufSize];//Allocating enough size for the buffer to hold the JPEG tile.
            if (TMDLLWrapper.GetTileImageAsRawJPG(zoom, x, y, newBuf, iBufSize, out iRetSize) == 0)//checks whether any fault has occured in loading the tile.if fault occurs 0 is returned else 1.
            {
                throw new FaultException("Could not get tile -zoom , x or y are probably out of range");
            }
            return newBuf;
        }

        ~TMDataControllerImpl()
        {
            Console.WriteLine("Client disconnected from the Server");
        }
    }
}
