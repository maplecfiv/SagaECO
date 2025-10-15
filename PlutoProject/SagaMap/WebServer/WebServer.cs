using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using SagaLib;

namespace SagaMap.WebServer
{
    public class WebServer
    {
        private static readonly Serilog.Core.Logger _logger = Logger.InitLogger<WebServer>();
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _responderMethod;
        private bool data;
        private string stoken = Configuration.Configuration.Instance.APIKey;
        private bool success;

        public WebServer(string[] prefixes, Func<HttpListenerRequest, string> method)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Cannot Start Server, APIServer requires at lease Windows XP SP2 or Windows Server 2003.");

            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("Error: No prefixes set.");

            if (method == null)
                throw new ArgumentException("Error: No method is set");

            foreach (var s in prefixes)
                _listener.Prefixes.Add(s);

            _responderMethod = method;
            _listener.Start();
        }

        public WebServer(Func<HttpListenerRequest, string> method, params string[] prefixes)
            : this(prefixes, method)
        {
        }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                try
                {
                    while (_listener.IsListening)
                        ThreadPool.QueueUserWorkItem(c =>
                        {
                            var ctx = c as HttpListenerContext;
                            if (ctx.Request.HttpMethod == "POST")
                            {
                                //Allow POST to connect
                                Logger.getLogger()
                                    .Information("Client connected:" + ctx.Request.RemoteEndPoint.Address);

                                //Debug
                                //_logger.Debug(ctx.Request.Headers.Get("token"));
                                //_logger.Debug(ctx.Request.Headers.Get("char_id"));
                                //_logger.Debug(ctx.Request.Headers.Get("item_id"));
                                //_logger.Debug(ctx.Request.Headers.Get("qty"));
                                //_logger.Debug(ctx.Request.Headers.Get("action"));
                                var token = ctx.Request.Headers.Get("token");

                                if (token == Configuration.Configuration.Instance.APIKey)
                                {
                                    switch (ctx.Request.Headers.Get("action"))
                                    {
                                        case "vshop_buy":
                                            if (ctx.Request.Headers.Get("char_id") == null ||
                                                int.Parse(ctx.Request.Headers.Get("char_id")) <= 0)
                                            {
                                                Logger.getLogger().Warning("No char_id received");
                                                ctx.Response.OutputStream.Close();
                                            }

                                            var charid = uint.Parse(ctx.Request.Headers.Get("char_id"));
                                            var itemid = uint.Parse(ctx.Request.Headers.Get("item_id"));
                                            var qty = ushort.Parse(ctx.Request.Headers.Get("qty"));
                                            var p = new Process.Process();
                                            p.Action(charid, itemid, qty);
                                            success = p.Load();
                                            break;
                                        case "inv_query":
                                            if (ctx.Request.Headers.Get("char_id") == null ||
                                                int.Parse(ctx.Request.Headers.Get("char_id")) <= 0)
                                            {
                                                Logger.getLogger().Warning("No char_id received");
                                                ctx.Response.OutputStream.Close();
                                            }

                                            charid = uint.Parse(ctx.Request.Headers.Get("char_id"));
                                            var p2 = new Process.Process();
                                            p2.Query(charid);
                                            data = p2.InvQuery();
                                            if (data)
                                                success = p2.InvQuery();
                                            else
                                                success = false;
                                            break;
                                        case "announce":


                                            var body = ctx.Request.InputStream;
                                            var encoding = ctx.Request.ContentEncoding;
                                            var reader = new StreamReader(body, encoding);
                                            var s = reader.ReadToEnd();
                                            body.Close();
                                            reader.Close();

                                            var message = ctx.Request.Headers.Get("message");
                                            Logger.getLogger().Information("An announce has made. (" + s + ")");

                                            var p3 = new Process.Process();
                                            p3.Announce(message);
                                            success = true;
                                            break;
                                        default:
                                            Logger.getLogger().Warning("Action is empty or not exists.");

                                            break;
                                    }
                                }
                                else
                                {
                                    Logger.getLogger().Warning("Token access deined.");
                                    //Console.ForegroundColor = ConsoleColor.Red;
                                    _logger.Debug("Dropped.");
                                    ctx.Response.OutputStream.Close();
                                }
                            }
                            else
                            {
                                //Not allow to GET
                                Logger.getLogger().Warning("Method disallowed from:" + ctx.Request.UserHostAddress);
                                //Console.ForegroundColor = ConsoleColor.Red;
                                _logger.Debug("Dropped.");
                                ctx.Response.OutputStream.Close();
                            }


                            try
                            {
                                string rstr;
                                if (success)
                                    rstr = "{\"success\":1,\"created_time\":\"" + DateTime.Now + "\"}";
                                else
                                    rstr = "{\"success\":0,\"created_time\":\"" + DateTime.Now + "\"}";


                                var buf = Encoding.UTF8.GetBytes(rstr);
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch
                            {
                            }
                            finally
                            {
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                }
                catch
                {
                }
            });
        }
    }
}