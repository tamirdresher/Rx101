using System;
using System.Collections.Generic;

namespace PushMessages
{
    class InteractiveSocialNetworksManager
    {
        ISocialNetworkClient _facebook = new FakeFacebookClient();
        ISocialNetworkClient _twitter = new FakeTwitterClient();
        ISocialNetworkClient _linkedin = new FakeLinkedinClient();

        public event EventHandler<Message> MessageAvailable;
        public void LoadMessages(string hashtag)
        {
            var statuses = _facebook.Search(hashtag);
            NotifyMessages(statuses);
            var tweets = _twitter.Search(hashtag);
            NotifyMessages(tweets);
            var linkedinMsgs = _linkedin.Search(hashtag);
            NotifyMessages(linkedinMsgs);
        }

        private void NotifyMessages(IEnumerable<Message> messages)
        {
            foreach (var message in messages)
            {
                MessageAvailable?.Invoke(this, message);
            }
        }
    }
}