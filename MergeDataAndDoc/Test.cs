using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;

namespace MergeDataAndDoc
{
    [TestFixture]
    class Test
    {
        [Test]
        public void testMerge1()
        {
            StringReader template = new StringReader("購物清單\r\n${姓名}：${文具}\t${份數}。\r\n");
            StringReader data = new StringReader("姓名\t文具\t份數\r\n貝爾\t筆\t三枝\r\n萊納\t筆記本\t兩本\r\n");
            StringWriter writer = new StringWriter();

            Program.Merge(writer, template, data);

            Assert.That(writer.ToString(), Is.EqualTo("購物清單\r\n貝爾：筆\t三枝。\r\n購物清單\r\n萊納：筆記本\t兩本。\r\n"));
        }

        [Test]
        public void testMergeEmpty()
        {
            StringReader template = new StringReader("${姓名}為本校專任${科目}教授，任期${年數}。\r\n");
            StringReader data = new StringReader("姓名\t科目\t年數\r\n");
            StringWriter writer = new StringWriter();

            Program.Merge(writer, template, data);

            Assert.That(writer.ToString(), Is.EqualTo(""));
        }
       
    }
}
