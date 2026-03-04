# ACureForTheCrown

**Royal Cure**, "Cell" temalı bir Game Jam için geliştirilmiş bir kaynak yönetimi ve karar verme oyunudur. Tema iki farklı açıdan ele alınmıştır: biyolojik hücreler (**Kanser**) ve hapishane hücreleri (**Zindan**).

---

## 📖 Hikaye ve Konsept
Oyuncu, kızı tedavisi olmayan bir kanser türüne yakalanmış bir Kralın rolünü üstlenir. Bir çözüm bulmak için Kral, halkını ve uzmanları (Simyacılar, Witcher'lar, Doktorlar) tedavi önerileri sunmaya davet eder. Temel amaç, krallığın kaynaklarını etkili bir şekilde yönetirken kızının kanser barını %0'a düşürmektir.

## 🎮 Oynanış Mekanikleri
Oyun, karakterlerden gelen teklifleri değerlendirdiğiniz kart tabanlı bir etkileşim sistemine sahiptir.

### Karar Seçenekleri
* **Sağa Kaydır (Kabul Et):** Önerilen çözümü uygular. Genellikle altın tüketir ancak sağlık barlarını iyileştirebilir.
* **Sola Kaydır (Reddet):** Teklifi geri çevirir. Kaynakları korur ancak sosyal veya sağlık açısından olumsuz sonuçları olabilir.
* **Hapse At (Imprison):** Teklif sunan kişiyi zindana gönderir. Eğer teklif Kraliyetle dalga geçiyorsa veya tehlikeliyse, ekonomiyi ve onuru korumak için kişiyi "hücreye" kapatma mekanizmasıdır.

---

## 📊 Kaynak Yönetimi
Oyuncunun dengelemesi gereken beş ana gösterge vardır:

1.  **Cancer (Hedef):** Ana amaç bu barı %0'a indirmektir.
2.  **Mental:** Kızın veya kraliyet ailesinin psikolojik durumu.
3.  **Immune:** Kızın biyolojik direnci.
4.  **Resources (Altın):** Krallığın hazinesi.
5.  **Honor:** Halkın Kral hakkındaki algısı ve moral değerleri.

---

## 🛠️ Teknik Detaylar
* **Motor:** Unity
* **Dil:** C#
* **Platform:** PC / WebGL
* **Görsel Stil:** 2D El çizimi ve Pixel Art karışımı UI.

## 🕹️ Nasıl Oynanır?
1. Karakterin diyaloğunu ve getireceği maliyet/fayda oranını okuyun.
2. Üç eylemden birini seçin: **Kabul Et, Reddet veya Hapse At.**
3. Beş kaynak barı üzerindeki etkileri izleyin.
4. Kaynaklar biterse veya Onur barı sıfırlanırsa oyun kaybedilir.
5. Kanser barı %0'a ulaştığında kız iyileşir ve oyun kazanılır.
