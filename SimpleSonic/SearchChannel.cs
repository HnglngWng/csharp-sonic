
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SimpleSonic
{

    public class SearchChannel : Channel
    {
        public SearchChannel(String address, int port, string password,
                int connectionTimeout, int readTimeout) : base(address, port, password, connectionTimeout, readTimeout)
        {

            this.Start(Mode.Search);
        }

        public List<string> Query(string collection, string bucket, string terms,
                int? limit, int? offset)

        {
            this.Send(String.Format(
                    "%s %s %s \"%s\"%s%s",
                    SearchType.QUERY.ToString(),
                    collection,
                    bucket,
                    terms,
                    limit != null ? String.Format(" LIMIT(%d)", limit) : "",
                    offset != null ? String.Format(" OFFSET(%d)", offset) : ""
            ));

            String queryId = AssertPendingSearch();
            return AssertSearchResults(SearchType.QUERY, queryId);
        }

        public List<String> Query(String collection, String bucket, String terms)

        {
            return Query(collection, bucket, terms, null, null);
        }

        public List<String> Suggest(String collection, String bucket, String word,
                int? limit)

        {
            this.Send(String.Format(
                    "%s %s %s \"%s\"%s",
                    SearchType.SUGGEST.ToString(),
                    collection,
                    bucket,
                    word,
                    limit != null ? String.Format(" LIMIT(%d)", limit) : ""
            ));

            String searchId = AssertPendingSearch();
            return AssertSearchResults(SearchType.SUGGEST, searchId);
        }

        public List<String> suggest(String collection, String bucket, String word)

        {
            return Suggest(collection, bucket, word, null);
        }

        protected String AssertPendingSearch()
        {
            String line1 = this.ReadLine();
            string pattern = "^PENDING ([a-zA-Z0-9]+)$";
            Regex reg = new Regex(pattern);
            bool isMatch = reg.IsMatch(line1);
            if (!isMatch)
            {
                throw new SonicException("unexpected prompt: " + line1);
            }
            Match matche = reg.Match(line1);
            return matche.Groups[1].Value;
        }

        protected List<String> AssertSearchResults(SearchType searchType, String searchId)
        {
            String line2 = this.ReadLine();
            string pattern = "^EVENT " + searchType.ToString() + " " + searchId + " (.+)?$";
            Regex reg = new Regex(pattern);
            bool isMatch = reg.IsMatch(line2);
            if (!isMatch)
            {
                throw new SonicException("unexpected prompt: " + line2);
            }

            Match matcher2 = reg.Match(line2);
            if (matcher2.Groups.Count != 1)
            {
                return new List<string>();
            }
            else
            {
                String[] searchResults = matcher2.Groups[1].Value.Split(" ".ToCharArray());
                return new List<string>(searchResults);
            }
        }
    }
}