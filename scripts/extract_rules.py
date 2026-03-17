"""
extract_rules.py — Run this once to generate rules-chunks.json from your PDF.

Requirements:
    pip install pdfplumber

Usage:
    python scripts/extract_rules.py path/to/core-rules.pdf

Output:
    W40k_CheatSheet.Client/wwwroot/data/rules-chunks.json
"""

import sys, json, re
from pathlib import Path

try:
    import pdfplumber
except ImportError:
    sys.exit("Install pdfplumber first:  pip install pdfplumber")

# ── Config ────────────────────────────────────────────────────────────────────
CHUNK_SIZE  = 400   # target words per chunk
OVERLAP     = 40    # words carried over to next chunk for context continuity
OUTPUT_FILE = Path(__file__).parent.parent / \
              "W40k_CheatSheet.Client" / "wwwroot" / "data" / "rules-chunks.json"
# ─────────────────────────────────────────────────────────────────────────────

def extract_text(pdf_path: str) -> str:
    pages = []
    with pdfplumber.open(pdf_path) as pdf:
        for page in pdf.pages:
            text = page.extract_text()
            if text:
                pages.append(text)
    return "\n".join(pages)

def clean(text: str) -> str:
    text = re.sub(r'\s+', ' ', text)           # collapse whitespace
    text = re.sub(r'(\w)-\s+(\w)', r'\1\2', text)  # rejoin hyphenated line-breaks
    return text.strip()

def chunk(text: str, size: int, overlap: int) -> list[str]:
    words  = text.split()
    chunks = []
    i      = 0
    while i < len(words):
        chunk_words = words[i : i + size]
        chunks.append(" ".join(chunk_words))
        i += size - overlap
    return [c for c in chunks if len(c.split()) > 20]  # drop tiny tail chunks

def main():
    if len(sys.argv) < 2:
        sys.exit("Usage: python extract_rules.py <path-to-pdf>")

    pdf_path = sys.argv[1]
    print(f"Reading {pdf_path} …")
    raw   = extract_text(pdf_path)
    text  = clean(raw)
    chunks = chunk(text, CHUNK_SIZE, OVERLAP)

    output = [{"text": c} for c in chunks]
    OUTPUT_FILE.parent.mkdir(parents=True, exist_ok=True)
    OUTPUT_FILE.write_text(json.dumps(output, ensure_ascii=False, indent=2), encoding="utf-8")
    print(f"✅  {len(chunks)} chunks → {OUTPUT_FILE}")

if __name__ == "__main__":
    main()
