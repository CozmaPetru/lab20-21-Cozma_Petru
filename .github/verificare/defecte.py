# Date interne ale verificării. Nu modifica acest fișier.
import base64,json,os,re,zlib
_d=json.loads(zlib.decompress(base64.b64decode("eJyVVGFv2jAQ/SuWP9GqShNIA+00bSxhE9K6D6SrNtXVZJIrtZbYyHFGt9L/PiVnSoCAyqcI3r13vufze6YRvSLPNPToFbmjicrnIuMa6BmhYex2/cA5HSWPYs5zkAZCJY1WWQZ62fqvc4okel/xu+2agYV7bbDnBt7RLSsSavr7NccyhSfGnKR4NHm23Py5UllGIMtcaOCod7HfFquz5q8HD1pZbvcS4f4uPJZ/QBquo2k1GTyZleBy1W6nArUGra0868cEr1WXKywFwzMhLDjdQiEVBvg/nH3SPYxuk+caTKmtdG8LzERhLNHfggoDegaIBftpwXa/BtZvn9J3fYvvG/S1YrBf/HLnvFyj+TG6+8BK13N7Gj8EtLKVPOXljNvS6eHaDXPj7uHiptVx73CtyPlMSPQ39o+oDY44Q/9wbcJL62Y8eFPlyxmht8NJHU3DmhF6FCMFtxr3EzcNbwJtq3p8QkINhD7FncPtwmVB1/Dc9SNCwgXFt4v7YHfcvhM0D22pKBFSaq0Qiy+pXVO7cfbWceh6ovj7JB4243adYsV5e7glRSVQFkLOSARzLrmGr3zqXKsUsuIdPSNGl9BI27dKCmmIUYZn5D1xK50HnhXQyOW3CjnDtAOv+MmGEq7brYDFhsZ5nb02eSuNj8JA7qyjt6lx0dD4VX3G+VxpUzTp+/xp2hMcNVXEDdyIHJxvauH8BK6dGxUbLeSsszkibn7EDT/fzfDm7V2LRKtCPRhnJI0wfz9rnsNC6d+h0rBxkYOjTjoZfRn9uJqX00wkZDxMjFByAkWZGRJhDDLWqW5bpIyd3DFWMBbfn36on68klaOMdVb9X17+A5rVaVs=")))
D=_d["D"];VAR=_d["VAR"];SURSA=_d["SURSA"]
def reparat_in_sursa(i,cale):
    f,t,prezent=SURSA[i]
    try: s=open(os.path.join(cale,f),encoding='utf-8').read()
    except OSError: return False
    if t.startswith('REGEX:'):
        m=re.search(r'public IActionResult Detalii\(int id\)\s*\{(.*?)\n        \}',s,re.S);return bool(m and 'return View' in m.group(1))
    return (t in s)==prezent

