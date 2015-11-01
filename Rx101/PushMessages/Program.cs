using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushMessages
{
    class Program
    {
        static void Main(string[] args)
        {


        }
    }

    class SocialNetworksManager
    {
        ISocialNetworkClient _facebook = new FakeFacebookClient();
        ISocialNetworkClient _twitter = new FakeTwitterClient();
        ISocialNetworkClient _linkedin = new FakeLinkedinClient();
        IEnumerable<Message> LoadMessages(string hashtag)
        {
            var statuses = _facebook.Search(hashtag);
            var tweets = _twitter.Search(hashtag);
            var linkedinMsgs = _linkedin.Search(hashtag);
            return statuses.Concat(tweets).Concat(linkedinMsgs);
        }

    }

    class Message
    {
        public string Content { get; set; }
    }

    interface ISocialNetworkClient
    {
        IEnumerable<Message> Search(string hashtag);
    }

    class FakeFacebookClient : ISocialNetworkClient
    {
        public IEnumerable<Message> Search(string hashtag)
        {
            return Enumerable.Range(1, 10).Select(i => "Status" + i).Select(m=>new Message() {Content = m});
        }
    }

    class FakeTwitterClient : ISocialNetworkClient
    {
        public IEnumerable<Message> Search(string hashtag)
        {
            return Enumerable.Range(1, 10).Select(i => "Tweet" + i).Select(m => new Message() { Content = m });

        }
    }

    class FakeLinkedinClient : ISocialNetworkClient
    {
        public IEnumerable<Message> Search(string hashtag)
        {
            return Enumerable.Range(1, 10).Select(i => "Update" + i).Select(m => new Message() { Content = m });

        }
    }
}
