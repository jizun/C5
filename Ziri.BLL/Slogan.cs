using System;
using System.Linq;
using System.Collections.Generic;
using Ziri.DAL;

namespace Ziri.BLL
{
    public class Slogan
    {
        public static MDL.Slogan GetRandom()
        {
            using (var EF = new EF())
            {
                var slogans = EF.Slogans.OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();
                if (slogans == null)
                {
                    InitSlogan();
                    return new MDL.Slogan { Text = "欢迎语已初始化完成。" };
                }
                return slogans;
            }
        }

        public static void InitSlogan()
        {
            using (var EF = new EF())
            {
                EF.Slogans.AddRange(
                    new List<MDL.Slogan> {
                        new MDL.Slogan{ Text="保重身体，就是保持良好的健康状况，而良好的健康状况有赖于身心的平衡，充沛的动力和完全平和的心态。即一种任何事情都扰乱不了的心理状态。"},
                        new MDL.Slogan{ Text="必须能探测顾客的心理，然后将之归纳为各种类型，再针对各种类型的特性，选择适当的商品方法和技巧。"},
                        new MDL.Slogan{ Text="不吃饭，不睡觉，打起精神赚钞票！"},
                        new MDL.Slogan{ Text="不管你的工作是怎样的卑微，都当付之以艺术家的精神，当有十二分的热忱。"},
                        new MDL.Slogan{ Text="不管你对于每天接触的客户具有何种想法，这都无所谓，重要的是你对待他们的方法。"},
                        new MDL.Slogan{ Text="不管你是多么擅长说服顾客购买许多东西，也必须让顾客如其所愿，照付那些金额才行，否则便不能说是一位优秀的推销员了。"},
                        new MDL.Slogan{ Text="不要满足于尚可的工作表现，要做最好的，你才能成为不可或缺的人物。"},
                        new MDL.Slogan{ Text="不要说谎，但可以在它被欣赏的地方说谎"},
                        new MDL.Slogan{ Text="成功的人千方百计，失败的人千难万险。"},
                        new MDL.Slogan{ Text="成功决不容易，还要加倍努力！"},
                        new MDL.Slogan{ Text="成功者懂得：自动自发的做事，同时为自己的所作所为承担责任。"},
                        new MDL.Slogan{ Text="承认自己的恐惧毫不可耻，可耻的是你因害怕而裹足不前。"},
                        new MDL.Slogan{ Text="创一流业绩，树驰名品牌。"},
                        new MDL.Slogan{ Text="大家好，才是真的好。"},
                        new MDL.Slogan{ Text="大踏步，大发展；人有多大劲，地有多大产。"},
                        new MDL.Slogan{ Text="当代价来的时候，那种满足感真正是无以伦比的美妙，但是绝没人说恒心是简单的。"},
                        new MDL.Slogan{ Text="当对方越冷淡时，你就越以明朗，动人的笑声对待他，这样一来你在气势上会居于优势，容易击倒对方。"},
                        new MDL.Slogan{ Text="当额外的工作分配到你头上时，不妨视之为一种机遇。"},
                        new MDL.Slogan{ Text="道路是曲折的，“钱”途无限光明！"},
                        new MDL.Slogan{ Text="对渴望成功者而言：拖延和逃避是最具破坏性，最危险的恶习。"},
                        new MDL.Slogan{ Text="多见一个客户就多一个机会！"},
                        new MDL.Slogan{ Text="付出一定会有回报。"},
                        new MDL.Slogan{ Text="复杂的事情简单化，简单的事情重复化。"},
                        new MDL.Slogan{ Text="个性迥异的两个人相安无事，其中之一必定有积极的心。"},
                        new MDL.Slogan{ Text="工作所给你的，要比你为它付出的更多。"},
                        new MDL.Slogan{ Text="行动的激励，方法决窍，行动知识，这三个因素是成功定律之钥。"},
                        new MDL.Slogan{ Text="行为本身并不能说明自身的性质，而是取决于我们行动时的精神状态。"},
                        new MDL.Slogan{ Text="机会包含于每个人的人格之中，正如未来的橡树包含在橡树果实里一样。"},
                        new MDL.Slogan{ Text="机会是为哪些有梦想和实施计划的人呈现。"},
                        new MDL.Slogan{ Text="激励别人采取行动的最好办法之一，是告诉他一个真实的故事。"},
                        new MDL.Slogan{ Text="激励的秘决，不只是诉之于道理，还要诉之于情感。"},
                        new MDL.Slogan{ Text="集中你的精力去学习一件小事，而成功一名专家；比你分散精力去学习许多小事要更加节省时间，而且容易成功。"},
                        new MDL.Slogan{ Text="加油吧，持续你的努力，每天，每月，积累一点一滴的进步，原本今天无法实现的梦想，明天，就可以得到丰硕的成果。"},
                        new MDL.Slogan{ Text="决窍，是以正确的方式，技巧，以及最少的时间和努力，去做好某件事情。"},
                        new MDL.Slogan{ Text="老板和员工并不是对立的，而是和谐统一的。"},
                        new MDL.Slogan{ Text="理智无法支配情绪，相反：行动才能改变情绪。"},
                        new MDL.Slogan{ Text="没有人不渴望被重视，也没有人不喜欢真诚的赞美，正确的评价会使对方“芳心大悦”。"},
                        new MDL.Slogan{ Text="每天进步一点点。"},
                        new MDL.Slogan{ Text="每一份私下的努力都会得到成倍的回报。"},
                        new MDL.Slogan{ Text="能够打破疆局，取悦客户的方法，就是善用人类疑虑，好奇，骄傲，趋利的个性。"},
                        new MDL.Slogan{ Text="你缺少的不是金钱，而是能力，经验和机会。"},
                        new MDL.Slogan{ Text="你现在所想的和所做的，将会决定你未来的命运。"},
                        new MDL.Slogan{ Text="你愈有恒心，你就会发现自己愈陷愈深，所以成功的推销自己，也就是不断地克服障碍。"},
                        new MDL.Slogan{ Text="你真的很不错！祝贺你今天的成功！相信你的明天会更好！"},
                        new MDL.Slogan{ Text="培养一点潇洒的习惯，不要太在意别人的看法或批评，如此你才能很自在的与他人相处。"},
                        new MDL.Slogan{ Text="人生的成功，不在于拿到一幅好牌，而是怎样将坏牌打好。"},
                        new MDL.Slogan{ Text="人是受环境影响的，因此，要主动选择最有益于向既定目标发展的环境。"},
                        new MDL.Slogan{ Text="任何人都抢不走你的无形资产——技能，经验，决心，信心。"},
                        new MDL.Slogan{ Text="日事日毕，日清日高。"},
                        new MDL.Slogan{ Text="如果你认为每一位成功者都只有成功的经验，那就错了。其实，没有比成功者拥有更多的是失败经验。"},
                        new MDL.Slogan{ Text="如果你想逃避某项事务，那么你就应该从这项事务着手，立即进行。"},
                        new MDL.Slogan{ Text="少数人需要智慧和勤奋，而多数人确要靠忠诚和勤奋。"},
                        new MDL.Slogan{ Text="社交场合中，任何人只要有喋喋不休的坏习惯，再好的朋友也会疏远他。"},
                        new MDL.Slogan{ Text="失败铺垫出来成功之路！"},
                        new MDL.Slogan{ Text="失败与挫折只是暂时的，成功已不会太遥远！"},
                        new MDL.Slogan{ Text="始终保持一种尽善尽美的工作态度，满怀希望和热情的朝着目标努力。"},
                        new MDL.Slogan{ Text="世界上有两种废物：一种是没有什么东西献给社会，另一种是不知怎样把他们己有的东西展示出来。"},
                        new MDL.Slogan{ Text="事实上，当你说谎的时候，你从来没有真正愚弄任何人。"},
                        new MDL.Slogan{ Text="事先写出自己所要提出的每点意见，以合乎逻辑的顺序表达出来：言简意骇，抓住重点。"},
                        new MDL.Slogan{ Text="所有的抱怨，不过是逃避责任的借口。"},
                        new MDL.Slogan{ Text="谈话要切题，谈话要有逻辑性，谈话要有力，谈话要充满自信，要使你推销时的谈话直接，自然，而且尽可能的简洁。"},
                        new MDL.Slogan{ Text="提问决定谈话，辨论，论证的方向。"},
                        new MDL.Slogan{ Text="听话是一种优雅的艺术，但很多人没有充分利用这种艺术，他们认为人有两只耳朵，所以肯定会知道如何去听。"},
                        new MDL.Slogan{ Text="团结一心，其利断金！团结一致，再创佳绩！"},
                        new MDL.Slogan{ Text="推销任何商品，只要秉持真诚，使对方坦诚相待，完全信赖，并非难事。"},
                        new MDL.Slogan{ Text="推销员接近顾客的方式，往往决定自己在他们心目中的地位是“接单者”还是“建议者”。"},
                        new MDL.Slogan{ Text="推销员要具有：良好的外在形象，行为举止；学会微笑；善用眼神；懂得交谈中的礼节。"},
                        new MDL.Slogan{ Text="推销员应该研究自己的洞察力，判断别人性格的能力。应该把研究别人和研究激励他们的动机作为重要的事情。"},
                        new MDL.Slogan{ Text="妥协不仅是“出卖”，更是一种“消耗”自己的方式，也就是说：你将不可能成功的推销自己。"},
                        new MDL.Slogan{ Text="无论你是否喜欢，我们的生活早己被时间所束缚，（感恩励志文章只是你的流程表未经计划或计划的很差罢了。"},
                        new MDL.Slogan{ Text="无论是否你得到定单，你都要给你潜在的顾客留下美好的印象，以便他对你有一个长久的回忆。"},
                        new MDL.Slogan{ Text="无论是一小步，还是一大步，都要带动人类的进步。"},
                        new MDL.Slogan{ Text="现在的努力并不是为了现在的回报，而是为了未来。"},
                        new MDL.Slogan{ Text="相信自己，相信伙伴。"},
                        new MDL.Slogan{ Text="相信自己能做到比努力本身更重要。"},
                        new MDL.Slogan{ Text="懈怠会引起无聊，无聊会导致懒散。"},
                        new MDL.Slogan{ Text="星光依旧灿烂，激情仍然燃烧。因为有梦想，所以我存在。你在你的领域里不惜青春，我在我的道路上不知疲倦。"},
                        new MDL.Slogan{ Text="雄关漫道真如铁，而今迈步从头越。"},
                        new MDL.Slogan{ Text="要享受“秘密情报网”的资讯，就必须让同行觉得你是一个愿意和人分享资源，发财机会，秘术高招的人。"},
                        new MDL.Slogan{ Text="一个面带诚挚而热情笑容的人，所到之处莫不受到欢迎，而愁容满面的人，则四处碰避。"},
                        new MDL.Slogan{ Text="一个恰当的时间，恰当的场合，一个简单的微笑可以制造奇迹。"},
                        new MDL.Slogan{ Text="一个人如果学会始终让自己的大脑充满积极，进取，乐观，愉快和有希望的想法，那么他就己经解决了人生的一大奥秘。"},
                        new MDL.Slogan{ Text="一个人外在的形象，反映出他特殊的内涵，倘若别人不信任你的外表，你就无法成功的推销自己。"},
                        new MDL.Slogan{ Text="一个推销员做事的下下之策是：绕着真实四周耍把戏，渲染它或歪曲它。"},
                        new MDL.Slogan{ Text="因为有缘我们相聚，成功要靠大家努力！"},
                        new MDL.Slogan{ Text="因为自信，所以成功。"},
                        new MDL.Slogan{ Text="永不言退，我们是最好的团队。"},
                        new MDL.Slogan{ Text="仔细的整理仪表，不但能给准客户良好的第一印象，而且能够培养自己正确的姿态。"},
                        new MDL.Slogan{ Text="在晤谈当中，不要想面面俱到而加入太多论点，也不要使重要变得暖昧不明，模棱两可，应事先找到谈话的侧重点，紧紧把握住，并好好发展它。"},
                        new MDL.Slogan{ Text="只要花一点时间：慎重的考虑和计划，然后果敢实施，定能解决一切工作的问题、难题。"},
                        new MDL.Slogan{ Text="只要强迫自己散发热情，一旦需要热心参与某种活动，便能立刻感到这股热情的力量，进而勇往直前。"},
                        new MDL.Slogan{ Text="只要有勇气在众人面前说话，就有勇气私下与陌生人进行谈话，不论对方是何许的达官显贵。"},
                        new MDL.Slogan{ Text="只有尝试的人才会获得成功：尝试不会失去什么，如果尝试成功，就可以获得很多东西，所以尽管去尝试。"},
                        new MDL.Slogan{ Text="忠诚并不是从一而忠，而是一种职业的责任感。"},
                        new MDL.Slogan{ Text="忠诚合作，积极乐观，努力开拓，勇往直前。"},
                        new MDL.Slogan{ Text="众志成城飞越颠峰。"},
                        new MDL.Slogan{ Text="赚钱靠大家，幸福你我他。"},
                        new MDL.Slogan{ Text="纵使黑夜吞噬了一切，太阳还可以重新回来。"},
                        new MDL.Slogan{ Text="遵守一个诺言，可以使别人对你建立起信心，破坏你的诺言，不仅动摇了那个信心，同时可能伤了一个人的心。"},
                        new MDL.Slogan{ Text="昨天，是张作废的机票；明天，是尚未兑现的期票；只有今天，才是现金，才有流通的价值。"},
                        new MDL.Slogan{ Text="做为思想的主人，人们拥有力量，才智与爱。"},
                    });
                EF.SaveChanges();
            }
        }
    }
}
