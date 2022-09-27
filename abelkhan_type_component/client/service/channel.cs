using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using MessagePack;

namespace abelkhan
{
    public class channel : abelkhan.Ichannel
    {
        public delegate void onDisconnectHandle(channel ch);
        public event onDisconnectHandle onDisconnect;

        public delegate void DisconnectHandle(channel ch);
        public event DisconnectHandle Disconnect;

        public channel(Socket _s, string _host_port, uint xor_key, Queue<ArrayList> _que)
        {
            s = _s;
            host_port = _host_port;
            que = _que;

            xor_key0 = (byte)((xor_key >> 24) & 0xff);
            xor_key1 = (byte)((xor_key >> 16) & 0xff);
            xor_key2 = (byte)((xor_key >> 8) & 0xff);
            xor_key3 = (byte)(xor_key & 0xff);

            recvbuflength = 8 * 1024;
            recvbuf = new byte[recvbuflength];

            tmpbuf = null;
            tmpbufoffset = 0;

            _send_state = send_state.idel;
            send_buff = new Queue();

            try
            {
                s.BeginReceive(recvbuf, 0, recvbuflength, 0, new AsyncCallback(this.onRead), this);
            }
            catch (System.Net.Sockets.SocketException)
            {
                onDisconnect?.Invoke(this);
            }
        }

        public void setXorKey(uint xor_key)
        {
            xor_key0 = (byte)((xor_key >> 24) & 0xff);
            xor_key1 = (byte)((xor_key >> 16) & 0xff);
            xor_key2 = (byte)((xor_key >> 8) & 0xff);
            xor_key3 = (byte)(xor_key & 0xff);
        }

        public uint getXorKey()
        {
            return (uint)((xor_key0 << 24) | (xor_key1 << 16) | (xor_key2 << 8) | xor_key3);
        }

        public void disconnect()
        {
            s.Close();

            Disconnect?.Invoke(this);
        }

        private void onRead(IAsyncResult ar)
        {
            channel ch = ar.AsyncState as channel;

            try
            {
                int read = ch.s.EndReceive(ar);
                if (read > 0)
                {
                    MemoryStream st = new MemoryStream();
                    if (tmpbufoffset > 0)
                    {
                        st.Write(tmpbuf, 0, tmpbufoffset);
                    }
                    st.Write(recvbuf, 0, read);
                    st.Position = 0;
                    byte[] data = st.ToArray();
                    int data_len = tmpbufoffset + read;

                    tmpbuf = null;
                    tmpbufoffset = 0;

                    int offset = 0;
                    while (true)
                    {
                        int over_len = data_len - offset;
                        if (over_len < 4)
                        {
                            break;
                        }

                        int len = data[offset];
                        len |= data[offset + 1] << 8;
                        len |= data[offset + 2] << 16;
                        len |= data[offset + 3] << 24;

                        if (over_len < len + 4)
                        {
                            break;
                        }
                        offset += 4;

                        MemoryStream _tmp = new MemoryStream();
                        _tmp.Write(data, offset, len);
                        _tmp.Position = 0;
                        byte[] _tmp_data = _tmp.ToArray();
                        for (var i = 0; i < _tmp_data.Length; ++i)
                        {
                            var j = i % 4;
                            if (j == 0)
                            {
                                _tmp_data[i] ^= xor_key0;
                            }
                            else if (j == 1)
                            {
                                _tmp_data[i] ^= xor_key1;
                            }
                            else if (j == 2)
                            {
                                _tmp_data[i] ^= xor_key2;
                            }
                            else if (j == 3)
                            {
                                _tmp_data[i] ^= xor_key3;
                            }
                        }
                        dynamic unpackedObject = MessagePack.MessagePackSerializer.Deserialize<dynamic>(_tmp_data);
                        lock (que)
                        {
                            que.Enqueue(unpackedObject);
                        }

                        offset += len;
                    }

                    int overplus_len = data_len - offset;
                    st = new MemoryStream();
                    st.Write(data, offset, overplus_len);
                    st.Position = 0;
                    tmpbuf = st.ToArray();
                    tmpbufoffset = overplus_len;

                    ch.s.BeginReceive(recvbuf, 0, recvbuflength, 0,
                                      new AsyncCallback(this.onRead), this);
                }
                else
                {
                    ch.s.Close();

                    onDisconnect?.Invoke(this);
                }
            }
            catch (System.ObjectDisposedException)
            {
                onDisconnect?.Invoke(this);

            }
            catch (System.Net.Sockets.SocketException)
            {
                onDisconnect?.Invoke(this);
            }
        }

        public void push(ArrayList ev)
        {
            var _tmpdata = MessagePack.MessagePackSerializer.Serialize(ev);
            for (var i = 0; i < _tmpdata.Length; ++i)
            {
                var j = i % 4;
                if (j == 0)
                {
                    _tmpdata[i] ^= xor_key0;
                }
                else if (j == 1)
                {
                    _tmpdata[i] ^= xor_key1;
                }
                else if (j == 2)
                {
                    _tmpdata[i] ^= xor_key2;
                }
                else if (j == 3)
                {
                    _tmpdata[i] ^= xor_key3;
                }
            }
            var _tmplenght = _tmpdata.Length;

            var st = new MemoryStream();
            st.WriteByte((byte)(_tmplenght & 0xff));
            st.WriteByte((byte)((_tmplenght >> 8) & 0xff));
            st.WriteByte((byte)((_tmplenght >> 16) & 0xff));
            st.WriteByte((byte)((_tmplenght >> 24) & 0xff));
            st.Write(_tmpdata, 0, _tmpdata.Length);
            st.Position = 0;

            senddata(st.ToArray());
        }

        private void senddata(byte[] data)
        {
            try
            {
                lock (send_buff)
                {
                    if (_send_state == send_state.idel)
                    {
                        _send_state = send_state.busy;
                        tmp_send_buff = data;
                        send_len = 0;
                        s.BeginSend(data, 0, data.Length, SocketFlags.None,
                                    new AsyncCallback(this.send_callback), this);
                    }
                    else
                    {
                        send_buff.Enqueue(data);
                    }
                }
            }
            catch (System.ObjectDisposedException)
            {
                onDisconnect?.Invoke(this);

            }
            catch (System.Net.Sockets.SocketException)
            {
                onDisconnect?.Invoke(this);
            }
        }

        private void send_callback(IAsyncResult ar)
        {
            channel ch = ar.AsyncState as channel;

            try
            {
                int send = ch.s.EndSend(ar);
                lock (send_buff)
                {
                    send_len += send;
                    if (send_len < tmp_send_buff.Length)
                    {
                        s.BeginSend(tmp_send_buff, send_len, tmp_send_buff.Length - send_len,
                                    SocketFlags.None, new AsyncCallback(this.send_callback), this);
                    }
                    else if (send_len == tmp_send_buff.Length)
                    {
                        if (send_buff.Count <= 0)
                        {
                            _send_state = send_state.idel;
                        }
                        else
                        {
                            var data = (byte[])send_buff.Dequeue();
                            tmp_send_buff = data;
                            send_len = 0;
                            s.BeginSend(data, 0, data.Length, SocketFlags.None,
                                        new AsyncCallback(this.send_callback), this);
                        }
                    }
                }
            }
            catch (System.ObjectDisposedException)
            {
                onDisconnect?.Invoke(this);

            }
            catch (System.Net.Sockets.SocketException)
            {
                onDisconnect?.Invoke(this);
            }
        }

        public Socket s;
        public string host_port;

        private byte xor_key0;
        private byte xor_key1;
        private byte xor_key2;
        private byte xor_key3;

        private readonly byte[] recvbuf;
        private readonly int recvbuflength;

        private byte[] tmpbuf;
        private int tmpbufoffset;

        private readonly Queue send_buff;
        enum send_state
        {
            idel = 0,
            busy = 1,
        }
        private send_state _send_state;
        private byte[] tmp_send_buff;
        private int send_len;

        private readonly Queue<ArrayList> que;
    }
}

