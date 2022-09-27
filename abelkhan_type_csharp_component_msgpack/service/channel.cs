/*
 * channel
 * 2020/6/2
 * qianqians
 */
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using DotNetty.Transport.Channels;
using DotNetty.Buffers;

namespace abelkhan
{
    public class channel : abelkhan.Ichannel
    {
        private IChannelHandlerContext context;

        public channel_onrecv _channel_onrecv;

        public channel(IChannelHandlerContext _context)
        {
            context = _context;
            _channel_onrecv = new channel_onrecv();
        }

        public void disconnect()
        {
            context.CloseAsync();
        }

        public void push(ProtoRoot ev)
        {
            var _tmpdata = MessagePack.MessagePackSerializer.Serialize(ev);
            var _tmplenght = _tmpdata.Length;

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