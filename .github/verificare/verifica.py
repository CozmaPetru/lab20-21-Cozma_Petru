#!/usr/bin/env python3
# Verificarea automată a laboratoarelor 20 și 21. Rezultatul este informativ: nu dă nota.
import argparse,base64,glob,json,os,re,shutil,signal,socket,subprocess,sys,time
from playwright.sync_api import sync_playwright
PNG=base64.b64decode('iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNkYPhfDwAChwGA60e6kgAAAABJRU5ErkJggg==')
def numar(repo):
    nume=repo.split('/')[-1];return sum((i+1)*ord(c) for i,c in enumerate(nume))%1000
class R:
    def __init__(s): s.rez=[]
    def ok(s,sar,text,cond,motiv=''):
        s.rez.append((sar,text,bool(cond),motiv if not cond else ''));return bool(cond)
def rulare(cmd,cwd,timeout=400):
    env=dict(os.environ,DOTNET_NOLOGO='1',DOTNET_CLI_TELEMETRY_OPTOUT='1');env['PATH']=env.get('PATH','')+':'+os.path.expanduser('~/.dotnet/tools')
    p=subprocess.run(cmd,shell=True,cwd=cwd,env=env,capture_output=True,text=True,timeout=timeout);return p.returncode,p.stdout+p.stderr
def port_liber():
    s=socket.socket();s.bind(('127.0.0.1',0));n=s.getsockname()[1];s.close();return n
class Aplicatie:
    """Compilează, creează baza nouă și pornește aplicația."""
    def __init__(s,cale,nume): s.cale=os.path.abspath(cale);s.nume=nume;s.proc=None;s.jurnal='';s.compilat=False;s.baza='';s.url=''
    def porneste(s):
        for f in glob.glob(s.cale+'/*.db*'): os.remove(f)
        shutil.rmtree(s.cale+'/wwwroot/incarcari',ignore_errors=True)
        c,o=rulare('dotnet build -v q --nologo',s.cale);s.jurnal=o;s.compilat=(c==0)
        if not s.compilat: return False
        c,o=rulare('dotnet ef database update --no-build',s.cale);s.baza=o
        dll=[d for d in glob.glob(s.cale+'/bin/Debug/net*/'+s.nume+'.dll')][0]
        n=port_liber();s.url=f'http://127.0.0.1:{n}'
        env=dict(os.environ,ASPNETCORE_ENVIRONMENT='Development',ASPNETCORE_URLS=s.url,DOTNET_NOLOGO='1')
        env.pop('ASPNETCORE_HTTPS_PORTS',None)
        s.proc=subprocess.Popen(['dotnet',dll],cwd=s.cale,env=env,stdout=subprocess.DEVNULL,stderr=subprocess.DEVNULL)
        for _ in range(100):
            try:
                socket.create_connection(('127.0.0.1',n),timeout=0.3).close();return True
            except OSError: time.sleep(0.2)
        return False
    def opreste(s):
        if s.proc: s.proc.terminate();s.proc.wait(timeout=10);s.proc=None
def pagina_ok(pg,url):
    r=pg.goto(url);return r.status if r else 0
def erori_ms(pg):
    m=re.search(r'Erori în ModelState: (\d+)',pg.content());return int(m.group(1)) if m else -1
def asteapta(pg):
    pg.wait_for_load_state()
# ---------- Laboratorul 20 ----------
def lab20(a,repo,r,pg):
    N=numar(repo);U=a.url;cod='V%03d'%N
    nume=["101","102","103","201","202","203","301","302","401","402"]
    tarif=[400+((N+i*7)%9)*50 for i in range(10)];libera=[(N+i)%3!=0 for i in range(10)];etaj=[int(x[0]) for x in nume]
    st=pagina_ok(pg,U+'/Camere')
    r.ok('Pregătirea','Lista camerelor se deschide',st==200,'pagina /Camere nu merge: ai rulat Update-Database?')
    if st==200: r.ok('Pregătirea','Varianta din subsol este a ta',cod in pg.content(),'în subsol trebuie să apară '+cod)
    st=pagina_ok(pg,U+'/Camere/Cauta')
    if not r.ok('S1','Pagina de căutare se deschide',st==200,'adresa /Camere/Cauta dă codul %s'%st): pass
    else:
        def n(q):
            pg.goto(U+'/Camere/Cauta'+q);return int(pg.inner_text('#numar'))
        r.ok('S1','Fără parametri apar toate camerele',n('')==10,'fără parametri trebuie să apară 10 camere')
        r.ok('S1','Filtrul după etaj',n('?etaj=2')==3,'la etajul 2 sunt 3 camere')
        m=sorted(tarif)[4]
        r.ok('S1','Filtrul după tariful maxim',n('?tarifMaxim=%d'%m)==sum(1 for t in tarif if t<=m),'tariful maxim %d nu filtrează corect'%m)
        r.ok('S1','Doar camerele libere',n('?doarLibere=true')==sum(libera),'filtrul „doar libere” nu este corect')
        r.ok('S1','Filtrele lucrează împreună',n('?etaj=2&doarLibere=true')==sum(1 for e,l in zip(etaj,libera) if e==2 and l),'etajul și „doar libere” trebuie aplicate împreună')
        c=n('?etaj=abc')
        r.ok('S1','Valoarea greșită nu oprește pagina',c==10 and erori_ms(pg)==1,'cu etaj=abc trebuie: toate camerele și o eroare în ModelState. Parametrul este opțional?')
    pg.goto(U+'/Cereri/Noua')
    def completeaza(st='Ana Rusu',tu='Ion Lungu'):
        pg.select_option('[name=CameraId]',index=1)
        for k,v in {'Student.Nume':st,'Student.Telefon':'069111222','Student.Adresa.Oras':'Cahul','Student.Adresa.Strada':'Republicii 5','Tutore.Nume':tu,'Tutore.Telefon':'069333444','Tutore.Adresa.Oras':'Comrat','Tutore.Adresa.Strada':'Lenin 12'}.items():
            pg.fill(f'[name="{k}"]',v)
    completeaza();pg.click('button:has-text("Trimite cererea")');asteapta(pg)
    ok=pg.url.rstrip('/').endswith('/Cereri') and 'Ana Rusu' in pg.content()
    r.ok('S2','Cererea corectă apare în listă',ok,'după „Trimite cererea” trebuie să ajungi la /Cereri, cu cererea în listă')
    if ok: r.ok('S2','Orașul din obiectul imbricat a ajuns în tabel','Cahul' in pg.inner_text('#cereri') and 'Republicii 5' in pg.inner_text('#cereri'),'orașul și strada lipsesc din tabel')
    pg.goto(U+'/Cereri/Noua');completeaza(st='');
    resp=None
    pg.click('button:has-text("Trimite cererea")');asteapta(pg)
    ct=pg.content()
    r.ok('S2','Cererea fără nume rămâne pe formular, cu lista de camere','Scrie numele' in ct and pg.locator('[name=CameraId] option').count()==10,'cu numele gol trebuie să rămâi pe formular, cu mesaj și cu lista camerelor (PregatesteCamerele)')
    pg.goto(U+'/Cereri/Noua');completeaza(st='Student Unu',tu='Tutore Doi');pg.click('#btnTutore');asteapta(pg)
    t=pg.locator('#numeTutore');nt=t.inner_text() if t.count() else ''
    r.ok('S3','Butonul arată datele tutorelui',nt=='Tutore Doi','pagina trebuie să arate numele tutorelui, nu al studentului (găsit: „%s”)'%nt)
    st=pagina_ok(pg,U+'/Camere/Tarife');cn=pg.locator('input[name$="].Tarif"]').count()
    if r.ok('S4','Tabelul are un câmp de tarif pe fiecare rând, cu index',cn==10,'câmpurile trebuie să fie [0].Tarif, [1].Tarif...; am găsit %d'%cn):
        pg.fill('[name="[0].Tarif"]','777,00');pg.fill('[name="[1].Tarif"]','888,00');pg.click('#formTarife button');asteapta(pg)
        pg.goto(U+'/Camere');x=pg.locator('#camere tbody tr td.tarif').all_inner_texts()
        r.ok('S4','Tarifele noi rămân salvate',len(x)>1 and x[0].startswith('777') and x[1].startswith('888'),'după salvare, primele două tarife trebuie să fie noile valori')
        pg.goto(U+'/Camere/Tarife');pg.fill('[name="[2].Tarif"]','-5');pg.click('#formTarife button');asteapta(pg)
        r.ok('S4','Tariful negativ dă mesaj','pozitiv' in pg.content(),'un tarif negativ trebuie să rămână pe pagină, cu mesajul de eroare')
    else: r.ok('S4','Tarifele noi rămân salvate',False,'tabelul nu este gata')
# ---------- Laboratorul 21 ----------
def eroare_vizibila(pg):
    e=pg.locator('#eroare')
    return e.count()>0 and e.first.inner_text().strip()!=''
def fis(nume,date=PNG,tip='image/png'): return {'name':nume,'mimeType':tip,'buffer':date}
def incarca(pg,U,id,f):
    r_=pg.goto(f'{U}/Lucrari/Incarca/{id}')
    if f is not None: pg.set_input_files('input[type=file]',f)
    pg.click('button:has-text("Încarcă")');asteapta(pg)
def lab21(a,repo,r,pg):
    N=numar(repo);U=a.url;cod='V%03d'%N;lim=1+N%3;sup='.gif' if N%2==0 else '.webp'
    st=pagina_ok(pg,U+'/Lucrari')
    r.ok('Pregătirea','Lista lucrărilor se deschide',st==200,'pagina /Lucrari nu merge: ai rulat Update-Database?')
    if st==200: r.ok('Pregătirea','Varianta din subsol este a ta',cod in pg.content(),'în subsol trebuie să apară '+cod)
    incarca(pg,U,1,fis('poza.png'));m=pg.locator('#mesaj');t=m.inner_text() if m.count() else ''
    if r.ok('S1','Fișierul se salvează, cu mesaj',('Detalii/1' in pg.url) and 'salvat' in t,'după încărcare trebuie să ajungi la Detalii, cu mesajul „Fișierul a fost salvat: ...”. Ai enctype pe formular?'):
        nn=t.split(':')[-1].strip();f=os.path.join(a.cale,'wwwroot','incarcari',nn)
        r.ok('S1','Fișierul există în wwwroot/incarcari',os.path.exists(f),'nu găsesc %s în wwwroot/incarcari'%nn)
        r.ok('S1','Numele îl alege aplicația',nn!='poza.png' and nn.endswith('.png'),'numele fișierului trebuie generat, cu extensia păstrată')
    pg.goto(U+'/Lucrari/Detalii/1');im=pg.locator('#imagine')
    if r.ok('S2','Imaginea apare pe pagina lucrării',im.count()==1,'pe Detalii trebuie să apară <img id="imagine">'):
        pg.wait_for_timeout(300);w=im.evaluate('e=>e.naturalWidth');r.ok('S2','Imaginea se încarcă din server',w>0,'adresa imaginii nu merge')
    pr=open(os.path.join(a.cale,'Program.cs'),encoding='utf-8').read()
    r.ok('S2','Program.cs are UseStaticFiles','UseStaticFiles' in pr,'fără UseStaticFiles, aplicația publicată dă 404 pentru imagini')
    incarca(pg,U,2,None);r.ok('S3','Fără fișier apare un mesaj',eroare_vizibila(pg) and 'Incarca' in pg.url,'fără fișier trebuie să rămâi pe formular, cu mesaj')
    incarca(pg,U,2,fis('mare.png',b'\0'*(lim*1024*1024+10)));r.ok('S3','Fișierul prea mare este refuzat','Incarca' in pg.url and eroare_vizibila(pg),'limita ta este %d MB'%lim)
    incarca(pg,U,2,fis('sub.png',b'\0'*(lim*1024*1024-1000)));r.ok('S3','Fișierul sub limită este primit','Detalii/2' in pg.url,'un fișier sub %d MB trebuie primit'%lim)
    incarca(pg,U,3,fis('x.exe'));r.ok('S3','Extensia nepermisă este refuzată','Incarca' in pg.url and eroare_vizibila(pg),'.exe trebuie refuzat')
    alta='.webp' if sup=='.gif' else '.gif'
    incarca(pg,U,3,fis('x'+alta));r.ok('S3','Extensia altei variante este refuzată','Incarca' in pg.url,alta+' nu este permisă în varianta ta')
    incarca(pg,U,3,fis('x'+sup));r.ok('S3','Extensia ta suplimentară este primită','Detalii/3' in pg.url,sup+' trebuie primit')
    incarca(pg,U,4,fis('X.PNG'));r.ok('S3','Extensia cu litere mari este primită','Detalii/4' in pg.url,'compară extensia scrisă cu litere mici')
# ---------- Depanarea ----------
def teste_depanare(a,pg):
    U=a.url;T={}
    def st(u):
        x=pg.goto(U+u);return x.status if x else 0
    T['start']=st('/')==200
    T['lista']=st('/Echipamente')==200 and pg.locator('#lista tbody tr').count()>=6
    T['cauta']=False
    if st('/Echipamente/Cauta')==200:
        n1=int(pg.inner_text('#numar'));st('/Echipamente/Cauta?pretMaxim=1000');n2=int(pg.inner_text('#numar'));T['cauta']=(n1>=6 and n2==1)
    T['detalii']=st('/Echipamente/Detalii/1')==200 and pg.locator('#titlu').count()==1
    T['detalii404']=st('/Echipamente/Detalii/999')==404
    T['editeaza404']=st('/Echipamente/Editeaza/999')==404
    T['adauga']=False
    if st('/Echipamente/Adauga')==200:
        pg.fill('[name=Denumire]','Test Adaugare');pg.fill('[name=Serie]','T1');pg.fill('[name=Pret]','100,00');pg.click('button:has-text("Salvează")');asteapta(pg)
        T['adauga']=st('/Echipamente')==200 and 'Test Adaugare' in pg.content()
    T['editeaza']=False
    if st('/Echipamente/Editeaza/2')==200 and pg.locator('[name=Denumire]').count():
        pg.fill('[name=Denumire]','Test Editare');pg.click('button:has-text("Salvează")');asteapta(pg)
        T['editeaza']=st('/Echipamente')==200 and 'Test Editare' in pg.content()
    T['preturi']=False
    if st('/Echipamente/Preturi')==200 and pg.locator('.pretNou').count()>=6:
        pg.locator('.pretNou').nth(2).fill('4321,00');pg.click('button:has-text("Salvează")');asteapta(pg)
        T['preturi']=st('/Echipamente')==200 and '4.321,00' in pg.content()
    T['imagine']=False
    if st('/Echipamente/Imagine/1')==200:
        pg.set_input_files('input[type=file]',fis('a.png'));pg.click('button:has-text("Încarcă")');asteapta(pg)
        im=pg.locator('#imagine')
        if 'Detalii/1' in pg.url and im.count()==1:
            pg.wait_for_timeout(200);T['imagine']=im.evaluate('e=>e.naturalWidth')>0
    T['sterge']=False
    if st('/Echipamente')==200:
        n=pg.locator('#lista tbody tr').count()
        if n: 
            pg.locator('#lista tbody tr').last.locator('button:has-text("Șterge")').click();asteapta(pg)
            T['sterge']=st('/Echipamente')==200 and pg.locator('#lista tbody tr').count()==n-1
    return T
NUME={'start':'Pagina de start se deschide','lista':'Lista echipamentelor se deschide','cauta':'Căutarea merge cu și fără parametri','detalii':'Pagina de detalii se deschide','detalii404':'Detalii pentru un echipament inexistent dă 404','editeaza404':'Editarea unui echipament inexistent dă 404','adauga':'Echipamentul adăugat apare în listă','editeaza':'Editarea se salvează','preturi':'Prețul schimbat în tabel se salvează','imagine':'Imaginea se încarcă și apare pe detalii','sterge':'Ștergerea funcționează'}
def depanare(root,repo,pw):
    sys.path.insert(0,os.path.dirname(os.path.abspath(__file__)))
    import defecte
    v='ABCD'[numar(repo)%4];cale=os.path.join(root,'Depanare',v,'DepanareLab')
    a=Aplicatie(cale,'DepanareLab');rez={'varianta':v,'compilat':False,'reparate':0,'total':7,'teste':{}}
    pornit=a.porneste();rez['compilat']=a.compilat
    fix=0;ids=defecte.VAR[v]
    if not a.compilat:
        # O eroare poate ascunde alta, deci citim și sursa.
        for i in ids:
            if defecte.D[i][0]=='compilare' and defecte.reparat_in_sursa(i,cale): fix+=1
        rez['erori']=sorted(set(re.findall(r'error (CS\d+)',a.jurnal)))
    else:
        fix+=sum(1 for i in ids if defecte.D[i][0]=='compilare')
        if pornit:
            b=pw.chromium.launch();c=b.new_context();pg=c.new_page();pg.set_default_timeout(15000)
            T=teste_depanare(a,pg);b.close();rez['teste']=T
            fix+=sum(1 for i in ids if defecte.D[i][0]!='compilare' and T.get(defecte.D[i][1]))
    a.opreste();rez['reparate']=fix;return rez
# ---------- cod și commit-uri ----------
def observatii(diff,lab):
    o=[];ad=[];f=''
    for l in diff.splitlines():
        if l.startswith('+++ '): f=l;continue
        # Migrațiile le generează Visual Studio; vederile folosesc => doar în codul dat gata.
        if l.startswith('+') and '/Migrations/' not in f: ad.append(l[1:])
    tx='\n'.join(l for l in ad if not l.strip().startswith('//'))
    if re.search(r'\.(Where|Select|OrderBy|FirstOrDefault|Any|Count)\(\s*\w+\s*=>|=>',tx): o.append('Cod cu lambda sau LINQ: la acest laborator nu l-am studiat încă. Va trebui să-l explici oral.')
    if re.search(r'\bvar\s+\w+\s*=',tx): o.append('Cod cu `var`. Va trebui să-l explici oral.')
    if re.search(r'\b(async|await)\b',tx): o.append('Cod cu `async`/`await`. Va trebui să-l explici oral.')
    return o
def main():
    ap=argparse.ArgumentParser();ap.add_argument('--lab',type=int,required=True);ap.add_argument('--root',default='.');ap.add_argument('--repo',required=True);ap.add_argument('--doar-depanare',action='store_true');ap.add_argument('--diff');ap.add_argument('--commituri');ap.add_argument('--rezumat');ap.add_argument('--json')
    A=ap.parse_args();root=os.path.abspath(A.root);r=R();rd=None;L=[]
    with sync_playwright() as pw:
        if not A.doar_depanare:
            app='CaminLab' if A.lab==20 else 'ExpozitieLab';a=Aplicatie(os.path.join(root,app),app)
            pornit=a.porneste()
            if r.ok('Compilare',app+' se compilează',a.compilat,'; '.join(sorted(set(re.findall(r'error (CS\d+[^\[]*)',a.jurnal)))[:3])) and r.ok('Pornire','Aplicația pornește',pornit,'aplicația nu pornește'):
                b=pw.chromium.launch();c=b.new_context();pg=c.new_page();pg.set_default_timeout(15000)
                try: (lab20 if A.lab==20 else lab21)(a,A.repo,r,pg)
                except Exception as e: r.ok('Verificare','Verificarea s-a terminat',False,str(e).splitlines()[0][:200])
                b.close()
            a.opreste()
        if A.lab==21: rd=depanare(root,A.repo,pw)
    L.append(f'## Verificarea automată · Laboratorul {A.lab} · `{A.repo}`\n')
    L.append('Rezultatul este informativ și nu dă nota.\n')
    if r.rez:
        L.append('| Etapa | Punctul de control | Rezultat | Motivul |\n|---|---|---|---|')
        for s,t,o,m in r.rez: L.append(f'| {s} | {t} | {"✅" if o else "❌"} | {m} |')
        L.append(f'\n**Trecute: {sum(1 for x in r.rez if x[2])} din {len(r.rez)}**\n')
    if rd:
        L.append(f'### Depanarea · proiectul {rd["varianta"]}\n\n**Defecte reparate: {rd["reparate"]} din {rd["total"]}**\n')
        if not rd['compilat']: L.append('Proiectul nu se compilează încă. Coduri de eroare: '+', '.join(rd.get('erori',[]))+'\n')
        if rd['teste']:
            L.append('| Comportament | Rezultat |\n|---|---|')
            for k,v in rd['teste'].items(): L.append(f'| {NUME[k]} | {"✅" if v else "❌"} |')
    if A.commituri and os.path.exists(A.commituri):
        cm=open(A.commituri,encoding='utf-8').read()
        lipsa=[s for s in (['S1','S2','S3','S4'] if A.lab==20 else ['S1','S2','S3','Fix']) if not re.search(r'^'+s+r'\b',cm,re.M)]
        if lipsa: L.append('\n**Commit-uri lipsă:** '+', '.join(lipsa))
    if A.diff and os.path.exists(A.diff):
        for o in observatii(open(A.diff,encoding='utf-8',errors='ignore').read(),A.lab): L.append('\n> '+o)
    tx='\n'.join(L)
    if A.rezumat: open(A.rezumat,'a',encoding='utf-8').write(tx+'\n')
    else: print(tx)
    if A.json: json.dump({'rezultate':r.rez,'depanare':rd},open(A.json,'w'),ensure_ascii=False)
    if rd: print('DEPANARE',rd['reparate'],rd['total'])
    esec=any(not x[2] for x in r.rez) or (rd and rd['reparate']<rd['total'])
    sys.exit(1 if esec else 0)
if __name__=='__main__': main()
