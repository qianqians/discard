/*
 * cryptchannel
 * qianqians
 * 2020/6/4
 */
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using DotNetty.Transport.Channels;
using DotNetty.Buffers;

namespace abelkhan
{
    public class cryptchannel : abelkhan.Ichannel
    {
        private IChannelHandlerContext context;
        private byte xor_key0;
        private byte xor_key1;
        private byte xor_key2;
        private byte xor_key3;

        public channel_onrecv _channel_onrecv;

        public cryptchannel(uint xor_key, IChannelHandlerContext _context)
        {
            set_xor_key(xor_key);

            context = _context;

            _channel_onrecv = new channel_onrecv();
            _channel_onrecv.on_recv_data += (byte[] data) => {
                for (var i = 0; i < data.Length; ++i)
                {
                    if ((i % 4) == 0)
                    {
                        data[i] ^= this.xor_key0;
                    }
                    else if ((i % 4) == 1)
                    {
                        data[i] ^= this.xor_key1;
                    }
                    else if ((i % 4) == 2)
                    {
                        data[i] ^= this.xor_key2;
                    }
                    else if ((i % 4) == 3)
                    {
                        data[i] ^= this.xor_key3;
                    }
                }
            };
        }

        public void set_xor_key(uint xor_key)
        {
            xor_key0 = (byte)((xor_key >> 24) & 0xff);
            xor_key1 = (byte)((xor_key >> 16) & 0xff);
            xor_key2 = (byte)((xor_key >> 8) & 0xff);
            xor_key3 = (byte)(xor_key & 0xff);
        }

        public void disconnect()
        {
            context.CloseAsync();
        }

        public void push(ProtoRoot ev)
        {
            var _tmpdata = MessagePack.MessagePackSerializer.Serialize(ev);
            var _tmplenght = _tmpdata.Length;
            for (var i = 0; i < _tmplenght; ++i)
            {
                if ((i % 4) == 0)
                {
                    _tmpdata[i] ^= this.xor_key0;
                }
                else if ((i % 4) == 1)
                {
                    _tmpdata[i] ^= this.xor_key1;
                }
                else if ((i % 4) == 2)
                {
                    _tmpdata[i] ^= this.xor_key2;
                }
                else if ((i % 4) == 3)
                {
                    _tmpdata[i] ^= this.xor_key3;
                }
            }

            var st = new MemoryStream();
            st.WriteByte((byte)(_tmplenght & 0xff));
            st.WriteByte((byte)((_tmplenght >> 8) & 0xff));
            st.WriteByte((byte)((_tmplenght >> 16) & 0xff));
            st.WriteByte((byte)((_tmplenght >> 24) & 0xff));
            st.Write(_tmpdata, 0, _tmpdata.Length);
            st.Position = 0;

            var len = st.Length;
            var initialMessage = Unpooled.Buffer((int)len);
            initialMessage.WriteBytes(st.ToArray());

            context.WriteAndFlushAsync(initialMessage);
        }
    }
}