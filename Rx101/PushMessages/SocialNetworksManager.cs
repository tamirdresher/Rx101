using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace PushMessages
{
    class SocialNetworksManager
    {
        ISocialNetworkClient _facebook = new FakeFacebookClient();
        ISocialNetworkClient _twitter = new FakeTwitterClient();
        ISocialNetworkClient _linkedin = new FakeLinkedinClient();
        public IEnumerable<Message> LoadMessages(string hashtag)
        {
            var statuses = _facebook.Search(hashtag);
            var tweets = _twitter.Search(hashtag);
            var linkedinMsgs = _linkedin.Search(hashtag);
            return statuses.Concat(tweets).Concat(linkedinMsgs);
        }
        
        public IObservable<Message> ObserveLoadedMessages(string hashtag)
        {
            return Observable.Merge(
                _facebook.ObserveSearchedMessages(hashtag),
                _twitter.ObserveSearchedMessages(hashtag),
                _linkedin.ObserveSearchedMessages(hashtag));

            //
            //The above can also be written like this:
            //
            //_facebook.ObserveSearchedMessages(hashtag)
            //    .Merge(_twitter.ObserveSearchedMessages(hashtag))
            //    .Merge(_twitter.ObserveSearchedMessages(_linkedin));
        } 

    }
}